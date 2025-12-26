using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nop.Core
{
    /// <summary>
    /// Common Enum to apply filteration on All/True/False
    /// </summary>
    public enum BooleanFilter
    {
        Both,
        True,
        False
    }

    public static class IQueryableExtensions
    {
        /// <summary>
        /// Conditionally applies a Where clause based on a boolean filter.<br />
        /// Example usage: <b>query = query.WhereBoolean(x => x.Deleted, deleted);</b>
        /// </summary>
        public static IQueryable<TSource> WhereBoolean<TSource>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> propertySelector,
            BooleanFilter filter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));

            // Don't apply any filter
            if (filter == BooleanFilter.Both)
                return source;

            var parameter = propertySelector.Parameters[0];
            Expression body = propertySelector.Body;

            if (filter == BooleanFilter.False)
            {
                body = Expression.Not(body);
            }

            var lambda = Expression.Lambda<Func<TSource, bool>>(body, parameter);

            return source.Where(lambda);
        }
    }
}