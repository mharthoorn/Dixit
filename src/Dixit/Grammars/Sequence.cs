using System.Collections.Generic;
using System.Linq;

namespace Ace
{
    public class Sequence : IGrammar
    {
        public string Name { get; }
        List<IGrammar> list;
        ISyntax whitespace;

        public Sequence(string name, IEnumerable<IGrammar> items, ISyntax whitespace)
        {
            this.Name = name;
            this.whitespace = whitespace;
            list = items.ToList();
        }


        public bool Parse(ref Lexer lexer, out Node node)
        {
            whitespace.Parse(ref lexer); // consume whitespace.

            node = new Node(this, lexer.Consume());
            

            foreach (var grammar in list)
            {
                whitespace.Parse(ref lexer);

                bool ok = grammar.Parse(ref lexer, out Node n);
                if (ok) node.Append(n); else return false;
            }
            return true;
        }

        public override string ToString()
        {
            return $"{Name}({nameof(Sequence)})";
        }
    }

}