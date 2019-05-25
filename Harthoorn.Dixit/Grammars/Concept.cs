using System;

namespace Harthoorn.Dixit
{
    public class Concept : IGrammar
    {
        public string Name { get; }

        internal Language Language;

        public Concept(Language language, string name)
        {
            this.Name = name;
            this.Language = language;
        }

        public IGrammar Grammar { get; internal set; }

        public bool Parse(ref Lexer lexer, out Node node)
        {
            node = new Node(this, lexer.Here);

            if (Grammar is null) throw new Exception($"Concept {Name} does not have a defined grammar.");

            bool ok = this.Grammar.Parse(ref lexer, out Node n);
            node.Append(n);
            return ok;
        }


        public override string ToString()
        {
            return $"{Name} (Concept)";
        }
    }
}