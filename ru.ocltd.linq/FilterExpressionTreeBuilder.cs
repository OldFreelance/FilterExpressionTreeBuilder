using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace ru.ocltd.linq
{
    public class FilterExpressionTreeBuilder
    {
        public static Expression<Func<T, bool>> Build<T>(params Object[] values) where T : class
        {
            Expression result = null;

            foreach (var member in typeof(T).GetProperties())
            {
                foreach (var value in values)
                {
                    Expression expression = CreateExpression<T>(value, member);
                    if (expression == null) continue;
                    result = result == null ? expression : Expression.Or(result, expression);
                }
            }

            return Expression.Lambda<Func<T, bool>>(result, Expression.Parameter(typeof(T), "i"));;
        }

        /// <summary>
        /// Создание выраженияЮ проверяющего на равенство аргументы
        /// </summary>
        /// <typeparam name="T">Тип структуры которой пренедлежит второй аргумент</typeparam>
        /// <param name="value">Первый аргумент</param>
        /// <param name="member">Второй аргумент</param>
        /// <returns>Результирующее выражение или null если сравнение не возможно</returns>
        private static Expression CreateExpression<T>(object value, PropertyInfo member)
        {
            //Если второй аргумент строка
            if (member.PropertyType == typeof(string))
                return CompareStrings<T>(value, member);

            //Если типы одинаковые
            if(value.GetType()==member.PropertyType)
                return CompareEqualTypes<T>(value, member);

            //Если сравниваются строка и гуид
            if (value is string && StringIsGuid(value.ToString()) && member.PropertyType == typeof(Guid))
                return CompareStringAndGuid<T>(value, member);

			//В остальных случаях. Разрешено преобразование только из следующих типов
            if ((value is string ) && (member.PropertyType != typeof(bool)))
                return TryToCompare<T>(value, member);

            return null;
        }

        /// <summary>
        /// Сравнение аргументов если второй(поле структуры) имеет тип string
        /// </summary>
        /// <typeparam name="T">Тип структуры которой пренедлежит второй аргумент</typeparam>
        /// <param name="value">Первый аргумент</param>
        /// <param name="member">Второй аргумент</param>
        /// <returns>Результирующее выражение</returns>
        private static Expression CompareStrings<T>(object value, PropertyInfo member)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "i");
            ConstantExpression constant = Expression.Constant(value.ToString(), typeof(string));
            return Expression.Call(Expression.MakeMemberAccess(param, member), typeof(String).GetMethod("Contains", new Type[] { typeof(string) }), constant);
        }

        /// <summary>
        /// Сравнение аргументов имеющих одинаковый тип
        /// </summary>
        /// <typeparam name="T">Тип структуры которой пренедлежит второй аргумен</typeparam>
        /// <param name="value">Первый аргумент</param>
        /// <param name="member">Второй аргумент</param>
        /// <returns>Результирующее выражение</returns>
        private static Expression CompareEqualTypes<T>(object value, PropertyInfo member)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "i");
            ConstantExpression constant = Expression.Constant(value, value.GetType());
            return Expression.Equal(Expression.MakeMemberAccess(param, member), constant);
        }

        private static Expression CompareStringAndGuid<T>(object value, PropertyInfo member)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "i");
            ConstantExpression constant = Expression.Constant(new Guid(value.ToString()), typeof(Guid));
            return Expression.Equal(Expression.MakeMemberAccess(param, member), constant);
        }

        /// <summary>
        /// Попытка сравнить аргументы если их типы разные
        /// </summary>
        /// <param name="value">Первый аргумент</param>
        /// <param name="member">Второй аргумент</param>
        /// <returns>Результирующее выражение</returns>
        /// <returns>Попытка привести тип первого аргумента к типу второго с помощью функции ChangeType</returns>
        private static Expression TryToCompare<T>(object value, PropertyInfo member)
        {
            try
            {
                return Expression.Equal(Expression.MakeMemberAccess(Expression.Parameter(typeof(T), "i"), member),
                    Expression.Constant(Convert.ChangeType(value, member.PropertyType), member.PropertyType));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Проверка является ли данная строка гуидом
        /// </summary>
        /// <param name="value">Строка</param>
        /// <returns>Результат проверки</returns>
        private static bool StringIsGuid(string value)
        {
            Guid guid;
            return Guid.TryParse(value, out guid);
        }
    }
}
