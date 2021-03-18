using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.UI.WebControls;

namespace Fonade.Negocio.Utilidades
{
    public static class SortExpressionConverter<T>
    {
        private static IDictionary<SortDirection, ISortExpression> expressions =
            new Dictionary<SortDirection, ISortExpression>
        {
        { SortDirection.Ascending, new OrderByAscendingSortExpression() },
        { SortDirection.Descending, new OrderByDescendingSortExpression() }
        };

        interface ISortExpression
        {
            Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> GetExpression();
        }

        class OrderByAscendingSortExpression : ISortExpression
        {
            public Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> GetExpression()
            {
                return (c, f) => c.OrderBy(f);
            }
        }

        class OrderByDescendingSortExpression : ISortExpression
        {
            public Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> GetExpression()
            {
                return (c, f) => c.OrderByDescending(f);
            }
        }

        public static Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>>
            Convert(SortDirection direction)
        {
            ISortExpression sortExpression = expressions[direction];
            return sortExpression.GetExpression();
        }
    }

    public static class SortLambdaBuilder<T>
    {
        public static Func<T, object> Build(string columnName, SortDirection direction)
        {
            // x
            ParameterExpression param = Expression.Parameter(typeof(T), "x");

            // x.ColumnName1.ColumnName2
            Expression property = columnName.Split('.')
                                            .Aggregate<string, Expression>
                                            (param, (c, m) => Expression.Property(c, m));

            // x => x.ColumnName1.ColumnName2
            Expression<Func<T, object>> lambda = Expression.Lambda<Func<T, object>>(
                Expression.Convert(property, typeof(object)),
                param);

            Func<T, object> func = lambda.Compile();
            return func;
        }
    }

    public static class CollectionExtensions
    {
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> collection,
               string columnName, SortDirection direction)
        {
            Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> expression =
                SortExpressionConverter<T>.Convert(direction);

            Func<T, object> lambda =
                SortLambdaBuilder<T>.Build(columnName, direction);

            IEnumerable<T> sorted = expression(collection, lambda);
            return sorted;
        }
    }
}
