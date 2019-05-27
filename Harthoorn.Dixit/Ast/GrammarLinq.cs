using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{
    public static class GrammarLinq
    {
        public static SyntaxNode Find(this SyntaxNode node, string name)
        {
            return node.Find(n => n.Grammar.Name == name);
        }

        public static SyntaxNode Find(this SyntaxNode node, IGrammar grammar)
        {
            return node.Find(n => n.Grammar.Name == grammar.Name);
        }

        public static SyntaxNode FirstChild(this SyntaxNode node)
        {
            return node.Children?.First();
        }

        public static IEnumerable<SyntaxNode> DeepSelect(this SyntaxNode node, IGrammar grammar)
        {
            return node.DeepSelect(n => n.Grammar.Name == grammar.Name);
        }

        public static IEnumerable<SyntaxNode> DeepSelect(this SyntaxNode node, IEnumerable<IGrammar> grammars)
        {
            var grammar = grammars.FirstOrDefault();
            var results = DeepSelect(node, grammar).ToList();
            var tail = grammars.Skip(1).ToList();
            if (tail.Any())
            {
                return results.SelectMany(n => n.DeepSelect(tail));
            }
            else
            {
                return results;
            }
        }

        public static IEnumerable<SyntaxNode> DeepSelect(this SyntaxNode node, params IGrammar[] grammars)
        {
            return node.DeepSelect((IEnumerable<IGrammar>)grammars);
        }

        public static IEnumerable<SyntaxNode> DeepSelect(this IEnumerable<SyntaxNode> nodes, params IGrammar[] grammars)
        {
            return nodes.SelectMany(n => n.DeepSelect(grammars));
        }

        public static IEnumerable<SyntaxNode> PathSelect(this SyntaxNode node, IEnumerable<IGrammar> grammars)
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

        public static IEnumerable<SyntaxNode> PathSelect(this SyntaxNode node, params IGrammar[] grammars)
        {
            return node.PathSelect((IEnumerable<IGrammar>)grammars);
        }

        public static IEnumerable<SyntaxNode> PathSelect(this IEnumerable<SyntaxNode> nodes, IEnumerable<IGrammar> grammars)
        {
            return nodes.SelectMany(n => n.PathSelect(grammars));
        }

        public static IEnumerable<SyntaxNode> PathSelect(this IEnumerable<SyntaxNode> nodes, params IGrammar[] grammars)
        {
            return nodes.PathSelect((IEnumerable<IGrammar>)grammars);
        }

        public static IEnumerable<SyntaxNode> WhereChildren(this SyntaxNode node, IGrammar grammar)
        {
            return node.Children.Where(n => n.Grammar.Name == grammar.Name);
        }

        public static IEnumerable<SyntaxNode> WhereChildren(this IEnumerable<SyntaxNode> nodes, IGrammar grammar)
        {
            return nodes.SelectMany(n => n.Children.Where(c => c.Grammar.Name == grammar.Name));
        }

        public static IEnumerable<string> Values(this IEnumerable<SyntaxNode> node)
        {
            return node.Select(n => n.Token.Text);
        }


        public static IEnumerable<SyntaxNode> RecursiveSelect(this SyntaxNode node, IGrammar grammar)
        {
            return node.RecursiveSelect(n => n.Grammar.Name == grammar.Name);
        }

        public static IEnumerable<(SyntaxNode, SyntaxNode)> Tuple(this IEnumerable<SyntaxNode> nodes, IGrammar a, IGrammar b)
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