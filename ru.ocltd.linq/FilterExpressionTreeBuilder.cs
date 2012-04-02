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

            foreach (var value in values)
            {
                foreach (var member in typeof (T).GetProperties())
                {
                    Expression expression = CreateExpression<T>(value, member);
                    if (expression == null) continue;
                    result = result == null ? expression : Expression.Or(result, expression);
                }
            }

            Expression<Func<T, bool>> ret=Expression.Lambda<Func<T, bool>>(result, Expression.Parameter(typeof(T), "i"));
            return ret;
        }

        private static Expression CreateExpression<T>(object value, PropertyInfo member)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "i");
            ConstantExpression constant = Expression.Constant(value, value.GetType());

            if(value.GetType()==member.PropertyType)
            {
                if(value is string)
                    return Expression.Call(Expression.MakeMemberAccess(param, member), typeof (String).GetMethod("Contains", new Type[] {typeof (string)}), constant);
                return Expression.Equal(Expression.MakeMemberAccess(param, member), constant);
            }

            ConstantExpression constant2 = null;
            try
            {
                constant2 = Expression.Constant(Convert.ChangeType(value, member.PropertyType), member.PropertyType);
            }
            catch (InvalidCastException e)
            {
                return null;
            }
            catch (FormatException e)
            {
                return null;
            }

            return Expression.Equal(Expression.MakeMemberAccess(param, member), constant2);
        }
    }
}
