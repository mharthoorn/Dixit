using System;
using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{

    public class SyntaxError
    {
        public SyntaxNode SyntaxNode;
        public string Expected;
        public string LineAndLocation;

    }

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

        public static SyntaxError GetError(this SyntaxNode root)
        {
            var node = root.GetErrorNode();
            if (node is null) return null;

            return new SyntaxError
            {
                SyntaxNode = node,
                Expected = node.Grammar.Name,
                LineAndLocation = node.DisplayErrorLocation()
            };
        }

        private static string GetLineString(Token token)
        {
            var text = token.File.Text;
            int start = token.Start, end = token.Start, location = token.Start;

            do  start--; while (start > 0 && text[start] != '\n' && text[start] != '\r');
            do end++; while (end < text.Length && text[end] != '\n' && text[end] != '\r');


            return text.Substring(start, end - start + 1).Insert(location - start, "|").Trim('\n', '\r');
        }

        public static string DisplayErrorLocation(this SyntaxNode error)
        {
            return GetLineString(error.Token);
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

        public static void MarkAsError(this SyntaxNode node)
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