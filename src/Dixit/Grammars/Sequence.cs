using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{
    public class Sequence : IGrammar
    {
        public string Name { get; }
        List<IGrammar> list;
        ISyntax whitespace;

        public Sequence(string name, ISyntax whitespace)
        {
            this.Name = name;
            this.whitespace = whitespace;
        }

        public IGrammar Define(IEnumerable<IGrammar> items)
        {
            this.list = items.ToList();
            return this;
        }


        public bool Parse(ref Lexer lexer, out Node node)
        {
            whitespace.Parse(ref lexer); // consume whitespace.

            node = new Node(this, lexer.Here);

            Lexer bookmark = lexer;

            foreach (var grammar in list)
            {
                whitespace.Parse(ref lexer);

                bool ok = grammar.Parse(ref lexer, out Node n);
                node.Append(n);
                if (!ok) return false;
            }

            return true;
        }


        public override string ToString()
        {
            return $"{Name} ({nameof(Sequence)})";
        }
    }

}