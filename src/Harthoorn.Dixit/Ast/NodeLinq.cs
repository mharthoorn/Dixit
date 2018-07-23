using System;
using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public static class NodeLinq
    {
        public static IEnumerable<Node> RecursiveSelect(this Node node, Predicate<Node> predicate)
        {
            var results = new List<Node>();
            node.Visit(addwhen);

            return results;

            void addwhen(Node n) { if (predicate(n)) results.Add(n); };
        }

        public static IEnumerable<Node> DeepSelect(this Node node, Predicate<Node> predicate)
        {
            if (node.Children is null) yield break;

            foreach (var child in node.Children)
            {
                if (predicate(child))
                {
                    yield return child;
                }
                else
                {
                    foreach (var offspring in child.DeepSelect(predicate)) yield return offspring;
                }
            }
        }

        public static Node Find(this Node node, Predicate<Node> predicate)
        {
            if (node.Children is null) return null;
            if (predicate(node)) return node;
            foreach (var n in node.Children)
            {
                var result = Find(n, predicate);
                if (!(result is null)) return result;
            }
            return null;
        }




    }


}