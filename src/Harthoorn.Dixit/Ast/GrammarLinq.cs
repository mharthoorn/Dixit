using System;
using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{
    public static class GrammarLinq
    {
        public static Node Find(this Node node, string name)
        {
            return node.Find(n => n.Grammar.Name == name);
        }

        public static Node Find(this Node node, IGrammar grammar)
        {
            return node.Find(n => n.Grammar.Name == grammar.Name);
        }

        public static IEnumerable<Node> DeepSelect(this Node node, IGrammar grammar)
        {
            return node.DeepSelect(n => n.Grammar.Name == grammar.Name);
        }

        public static IEnumerable<Node> DeepSelect(this Node node, IEnumerable<IGrammar> grammars)
        {
            var grammar = grammars.FirstOrDefault();
            var results = DeepSelect(node, grammar);
            var tail = grammars.Skip(1);
            if (tail.Count() > 0)
            {
                return results.SelectMany(n => n.DeepSelect(grammars.Skip(1)));
            }
            else
            {
                return results;
            }
        }

        public static IEnumerable<Node> DeepSelect(this Node node, params IGrammar[] grammars)
        {
            return node.DeepSelect((IEnumerable<IGrammar>)grammars);
        }

        public static IEnumerable<Node> DeepSelect(this IEnumerable<Node> nodes, params IGrammar[] grammars)
        {
            return nodes.SelectMany(n => n.DeepSelect(grammars));
        }

        public static IEnumerable<Node> PathSelect(this Node node, IEnumerable<IGrammar> grammars)
        {
            var grammar = grammars.FirstOrDefault();
            var results = node.WhereChildren(grammar);
            var tail = grammars.Skip(1);
            if (tail.Count() > 0)
            {
                return results.SelectMany(n => n.PathSelect(tail));
            }
            else
            {
                return results;
            }
        }

        public static IEnumerable<Node> PathSelect(this Node node, params IGrammar[] grammars)
        {
            return node.PathSelect((IEnumerable<IGrammar>)grammars);
        }

        public static IEnumerable<Node> PathSelect(this IEnumerable<Node> nodes, IEnumerable<IGrammar> grammars)
        {
            return nodes.SelectMany(n => n.PathSelect(grammars));
        }

        public static IEnumerable<Node> PathSelect(this IEnumerable<Node> nodes, params IGrammar[] grammars)
        {
            return nodes.PathSelect((IEnumerable<IGrammar>)grammars);
        }

        public static IEnumerable<Node> WhereChildren(this Node node, IGrammar grammar)
        {
            return node.Children.Where(n => n.Grammar.Name == grammar.Name);
        }

        public static IEnumerable<Node> WhereChildren(this IEnumerable<Node> nodes, IGrammar grammar)
        {
            return nodes.SelectMany(n => n.Children.Where(c => c.Grammar.Name == grammar.Name));
        }

        public static IEnumerable<string> Values(this IEnumerable<Node> node)
        {
            return node.Select(n => n.Token.Text);
        }


        public static IEnumerable<Node> Select(this Node node, IGrammar grammar)
        {
            return node.RecursiveSelect(n => n.Grammar.Name == grammar.Name);
        }

        public static IEnumerable<(Node, Node)> Tuple(this IEnumerable<Node> nodes, IGrammar a, IGrammar b)
        {
            foreach (var node in nodes)
            {
                var na = node.Find(a);
                var nb = node.Find(b);
                yield return (na, nb);
            }
        }
    }

   
}