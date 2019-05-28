using System;

namespace Harthoorn.Dixit
{
    public class Node : IGrammar
    {
        public string Name { get; }

        internal Language Language;

        public Node (Language language, string name)
        {
            this.Name = name;
            this.Language = language;
        }

        public IGrammar Grammar { get; internal set; }

        public bool Parse(ref Lexer lexer, out SyntaxNode node)
        {
            if (Grammar is null) throw new Exception($"Grammar {Name} does not have a defined grammar.");

            bool ok = this.Grammar.Parse(ref lexer, out node);
            this.Fold(node);

            return ok;
        }


        public override string ToString()
        {
            return $"{Name} (Node)";
        }
    }
}