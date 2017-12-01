using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{
    public class Any : IGrammar
    {
        public string Name { get; }
        List<IGrammar> items;
        ISyntax whitespace;

        public Any(string name, IEnumerable<IGrammar> items, ISyntax whitespace)
        {
            this.Name = name;
            this.whitespace = whitespace;
            this.items = items.ToList();
        }

        public bool Parse(ref Lexer lexer, out Node node)
        {
            whitespace.Parse(ref lexer); // consume whitespace.

            node = new Node(this, lexer.Consume());
            var lexerbm = lexer;

            foreach (var grammar in items)
            {
                lexer.Reset(lexerbm);

                bool ok = grammar.Parse(ref lexer, out Node n);
                if (ok)
                {
                    node.Append(n);
                    return true;
                }

            }
            node.State = State.Error;
            return false;
        }

        public override string ToString()
        {
            return $"{Name}({nameof(Any)})";
        }
    }

}