using System;
using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{

    public static class Nodes
    {
        
        public static SyntaxNode Create(IGrammar grammar, ISyntax syntax, Token token)
        {
            var state = token.IsValid ? State.Valid : State.Error;
            var node = new SyntaxNode(grammar, syntax, token, state);
            return node;
        }

        public static SyntaxNode GetErrorNode(this SyntaxNode root)
        {
            var descendants = root.Descendants().ToList();
            var errors = root.Descendants().Where(n => n.State == State.Error).ToList();
            var branch = errors.GetFarthest();

            var node = branch.FindDeepest(n => n.State == State.Error && n.Grammar is Concept);

            return node;
        }

        public static IEnumerable<SyntaxNode> Descendants(this SyntaxNode node)
        {
            if (node.Children == null) yield break;

            foreach(var n in node.Children)
            {
                 yield return n;
                 var descendants = n.Descendants();
                 foreach (var d in descendants)
                 {
                     yield return d;
                 }
            }
        }

        public static SyntaxNode GetFarthest(this IEnumerable<SyntaxNode> nodes)
        {
            SyntaxNode longest = null; int max = 0;
            foreach(var node in nodes)
            {
                var end = node?.End ?? 0;
                if (end > max)
                {
                    longest = node;
                    max = end;
                }
            }
            return longest;
        }

        public static SyntaxNode GetFarthest(params SyntaxNode[] nodes)
        {
            return GetFarthest((IEnumerable<SyntaxNode>)nodes);
        }

        public static SyntaxNode GetFarthestChild(this SyntaxNode node)
        {
            var nodes = node.Children;
            return GetFarthest(nodes);
        }

        public static void Error(this SyntaxNode node)
        {
            if (node is object) node.State = State.Error;
        }

        public static SyntaxNode GetChild(this SyntaxNode node, Predicate<SyntaxNode> predicate)
        {
            var nodes = node.Children.Where(c => predicate(c));
            return GetFarthest(nodes);
        }

        public static void Visit(this SyntaxNode node, Action<SyntaxNode> action)
        {
            action(node);
            if (node.Children == null) return;
            foreach(var n in node.Children) Visit(n, action);
        }

        public static void Visit<T>(this SyntaxNode node, Action<SyntaxNode, T> action, T covariable, Func<T, T> cofunc)
        {
            action(node, covariable);
            if (node.Children == null) return;
            foreach(var n in node.Children) Visit(n, action, cofunc(covariable), cofunc);
        }

        public static void Prune(this SyntaxNode node)
        {
            if (node.Children is null) return;

            node.Children = node.Children.Where(c => c.State != State.Dismissed).ToList();
            foreach (var c in node.Children) c.Prune();
        }
    }

   
}