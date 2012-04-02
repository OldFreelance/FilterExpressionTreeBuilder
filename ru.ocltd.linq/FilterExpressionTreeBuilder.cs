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
            MemberInfo[] members = typeof (T).GetMembers().Where(s=>s.MemberType == MemberTypes.Property).ToArray();

            Expression result = null;

            foreach (var value in values)
            {
                foreach (var member in members.Where(s => ((PropertyInfo)s).PropertyType==value.GetType()))
                {
                    ParameterExpression param = Expression.Parameter(typeof(T),"i");
                    ConstantExpression constant = Expression.Constant(value, value.GetType());
                    Expression expression = (value is string)
                        ? (Expression)Expression.Call(Expression.MakeMemberAccess(param, member), typeof(String).GetMethod("Contains", new Type[] { typeof(string) }), constant)
                        : (Expression)Expression.Equal(Expression.MakeMemberAccess(param, member), constant);

                    if (result == null)
                        result = expression;
                    else
                    {
                        result = Expression<Func<T, bool>>.Or(result, expression);
                    }
                }
            }

            Expression<Func<T, bool>> ret=Expression.Lambda<Func<T, bool>>(result, Expression.Parameter(typeof(T), "i"));
            return ret;
        }
    }
}
