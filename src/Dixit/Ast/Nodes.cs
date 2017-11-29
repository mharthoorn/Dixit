using System;
using System.Collections.Generic;
using System.Linq;

namespace Ace
{
    public static class Nodes
    {
        public static Node CreateRootSyntaxNode()
        {
            return new Node(null, null, Token.Empty());
        }

        public static Node Create(IGrammar grammar, ISyntax syntax, Token token)
        {
            var state = token.IsValid ? State.Good : State.Error;
            var node = new Node(grammar, syntax, token, state);
            return node;
        }

        public static IEnumerable<Node> GetErrors(this Node root)
        {
            return root.Descendants().Where(n => n.Token.IsValid == false);
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

        public static Node Find(this Node node, Predicate<Node> predicate)
        {
            if (node.Children is null) return null;
            if (predicate(node)) return node;
            foreach(var n in node.Children) 
            {
                var result = Find(n, predicate);
                if (!(result is null)) return result;
            }
            return null;
        }

        public static Node Find(this Node node, string name)
        {
            return node.Find(n => n.Grammar.Name == name);
        }

        public static Node Find(this Node node, IGrammar grammar)
        {
            return node.Find(n => n.Grammar.Name == grammar.Name);
        }

        public static IEnumerable<Node> Select(this Node node, Predicate<Node> predicate)
        {
            var results = new List<Node>();
            node.Visit(addwhen);

            return results;

            void addwhen(Node n) { if (predicate(n)) results.Add(n); };
        }

        public static IEnumerable<Node> Select(this Node node, IGrammar grammar)
        {
            return Select(node, n => n.Grammar.Name == grammar.Name);
        }

        static IEnumerable<Node> RecursiveDescend(this Node node, IEnumerable<IGrammar> grammars)
        {
            var grammar = grammars.FirstOrDefault();
            var results = Select(node, n => n.Grammar.Name == grammar.Name);
            var tail = grammars.Skip(1);
            if (tail.Count() > 0)
            {
                return results.SelectMany(n => n.RecursiveDescend(grammars.Skip(1)));
            }
            else 
            {
                return results;
            }
        }

        public static IEnumerable<Node> Descend(this Node node, params IGrammar[] grammars)
        {
            return node.RecursiveDescend(grammars);
        }

        public static IEnumerable<string> Values(this IEnumerable<Node> node)
        {
            return node.Select(n => n.Token.Text);
        }

        public static IEnumerable<(Node, Node)> Tuple(this IEnumerable<Node> nodes, IGrammar a, IGrammar b)
        {
            foreach(var node in nodes)
            {
                var na = node.Find(a);
                var nb = node.Find(b);
                yield return (na, nb);
            }
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
    }

  

   
}