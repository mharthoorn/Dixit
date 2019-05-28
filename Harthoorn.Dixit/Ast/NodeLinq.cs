using System;
using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public static class NodeLinq
    {
        public static IEnumerable<SyntaxNode> RecursiveSelect(this SyntaxNode node, Predicate<SyntaxNode> predicate)
        {
            var results = new List<SyntaxNode>();
            node.Visit(addwhen);

            return results;

            void addwhen(SyntaxNode n) { if (predicate(n)) results.Add(n); };
        }

        public static IEnumerable<SyntaxNode> DeepSelect(this SyntaxNode node, Predicate<SyntaxNode> predicate)
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

        public static SyntaxNode FindDeepest(this SyntaxNode root, Predicate<SyntaxNode> predicate)
        {
            var deepest = Dig(root, 0);
            return deepest.node ?? root;


            (SyntaxNode node, int depth) Dig(SyntaxNode node, int depth)
            {
                if (node.Children is null) return (null, 0);

                (SyntaxNode node, int depth) best = (null, depth);
                foreach (var n in node.Children)
                {
                    var result = Dig(n, depth+1);

                    if (result.node is SyntaxNode && (best.node is null || result.depth > best.depth))
                    {
                        best = result;
                    }
                }
                if (best.node is SyntaxNode) return best;

                return predicate(node) ? (node, depth) : (null, 0);
            }
        }

        public static SyntaxNode Find(this SyntaxNode node, Predicate<SyntaxNode> predicate)
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