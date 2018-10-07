using System;
using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{

    public static class Nodes
    {
        
        public static Node Create(IGrammar grammar, ISyntax syntax, Token token)
        {
            var state = token.IsValid ? State.Valid : State.Error;
            var node = new Node(grammar, syntax, token, state);
            return node;
        }

        public static Node GetExpect(this Node node)
        {
            var error = node.GetError();
            if (error is null) return null;

            var expect = error;
            while (expect.Grammar.ExpectingConcept == false && expect.Parent != null) expect = expect.Parent;
            return expect;
        }

        public static Node GetError(this Node root)
        {
            var errors = root.Descendants().Where(n => n.State == State.Error);
            return errors.Longest();
        }

        public static IEnumerable<Node> Descendants(this Node node)
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

        public static Node Encapsulate(this Node node, Lexer previous, Lexer current)
        {
            node.Token = Lexer.Encapsulate(previous, current);
            return node;
        }

        public static Node Longest(this IEnumerable<Node> nodes)
        {
            Node longest = null; int max = 0;
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

        public static Node GetLongest(params Node[] nodes)
        {
            return Longest(nodes);
        }

        public static void Visit(this Node node, Action<Node> action)
        {
            action(node);
            if (node.Children == null) return;
            foreach(var n in node.Children) Visit(n, action);
        }

        public static void Visit<T>(this Node node, Action<Node, T> action, T covariable, Func<T, T> cofunc)
        {
            action(node, covariable);
            if (node.Children == null) return;
            foreach(var n in node.Children) Visit(n, action, cofunc(covariable), cofunc);
        }

        public static void Prune(this Node node)
        {
            if (node.Children is null) return;

            node.Children = node.Children.Where(c => c.State == State.Valid).ToList();
            foreach (var c in node.Children) c.Prune();
        }
    }

   
}