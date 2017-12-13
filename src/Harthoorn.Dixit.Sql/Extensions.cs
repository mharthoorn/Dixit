using System.Collections.Generic;

namespace Harthoorn.Dixit.Sql
{
    public static class Extensions
    {
        public static IEnumerable<Filter> ToFilters(this IEnumerable<Node> range)
        {
            foreach (var node in range)
            {
                var filter = new Filter
                {
                    Name = node.Children[0].Token.Text,
                    Operator = node.Children[1].Children[0].Token.Text,
                    Value = node.Children[2].Token.Text
                };
                yield return filter;

            }
        }

    }

}