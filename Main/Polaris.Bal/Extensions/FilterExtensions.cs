using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Polaris.Bal.Extensions
{
    public static class FilterExtensions
    {
        public static IQueryable<T> AddEqualityCondition<T>
                                    (this IQueryable<T> queryable,
                                     FilterDefinition<T> filter, FilterValueDefinition filterValue) where T : IDataEntity
        {
            ParameterExpression pe = Expression.Parameter(typeof(T), "p");

            IQueryable<T> x = queryable.Where<T>(
              Expression.Lambda<Func<T, bool>>(
                Expression.Equal(Expression.Property(
                  pe,
                  filter.RelatedProperty),
                  Expression.Constant(filterValue.FilterValue, filterValue.FilterValueType),
                  false,
                  typeof(T).GetMethod("op_Equality")),
              new ParameterExpression[] { pe }));

            return (x);
        }

    }
}
