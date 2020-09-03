using System;
using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{

    public class SyntaxError
    {
        public SyntaxNode SyntaxNode;

        public SyntaxError(SyntaxNode node)
        {
            this.SyntaxNode = node;
        }
        
        public IGrammar Grammar => SyntaxNode?.Grammar;
        public string Expected => SyntaxNode?.Grammar?.Name;
        public int Location => SyntaxNode?.Start ?? 0;
        public string LineAndLocation => SyntaxNode?.DisplayErrorLocation();
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
            var errors = root.Descendants().Where(n => n.State == State.Error).ToList();
            var dismissals = root.Descendants().Where(n => n.State == State.Dismissed).ToList();

            var error = errors.BestOf((a, b) => a.Token.Start > b.Token.Start);
            var dismissal = dismissals.BestOf((a, b) => a.Token.Start > b.Token.Start);
            var errorPt = error?.Token.Start ?? -1;
            var dismissPt = dismissal?.Token.Start ?? -1;
            return (errorPt >= dismissPt) ? error : dismissal;


            //var node = branch.FindDeepest(n => n.State != State.Valid);

            //return branch;
        }

        public static SyntaxError GetError(this SyntaxNode root)
        {
            var node = root.GetErrorNode();
            if (node is null) return null;

            return new SyntaxError(node);
        }

        private static string GetLineString(Token token)
        {
            var text = token.File.Text;
            int start = token.Start, end = token.Start, location = token.Start;

            while (start > 0 && text[start] != '\n' && text[start] != '\r')
            { start--; }

            while (end < text.Length-1 && text[end] != '\n' && text[end] != '\r') 
            { end++; }


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

        public static SyntaxNode GetFarthestEnd(this IEnumerable<SyntaxNode> nodes)
        {
            SyntaxNode farthest = null; int max = 0;
            foreach(var node in nodes)
            {
                var end = node?.End ?? 0;
                if (end > max || farthest is null)
                {
                    farthest = node;
                    max = end;
                }
            }
            return farthest;
        }

        public static T BestOf<T>(this IEnumerable<T> items, Func<T, T, bool> test)
        {
            T best = default;
            bool first = true;
            foreach(T item in items)
            {
                if (first)
                {
                    best = item;
                    first = false;
                }
                else
                {
                    if (test(item, best)) best = item;
                }
            }
            return best;
        }

        public static SyntaxNode GetFarthestStart(this IEnumerable<SyntaxNode> nodes)
        {
            SyntaxNode farthest = null; int max = 0;
            foreach (var node in nodes)
            {
                var start = node?.Start ?? 0;
                if (start > max)
                {
                    farthest = node;
                    max = start;
                }
            }
            return farthest;
        }

        public static SyntaxNode GetFarthest(params SyntaxNode[] nodes)
        {
            return GetFarthestEnd((IEnumerable<SyntaxNode>)nodes);
        }

        public static SyntaxNode GetFarthestChild(this SyntaxNode node)
        {
            var nodes = node.Children;
            return GetFarthestEnd(nodes);
        }

       
        public static SyntaxNode GetChild(this SyntaxNode node, Predicate<SyntaxNode> predicate)
        {
            var nodes = node.Children.Where(c => predicate(c));
            return GetFarthestEnd(nodes);
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