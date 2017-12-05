using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{
    public class Any : IGrammar
    {
        public string Name { get; }
        IList<IGrammar> children { get; set; }
        ISyntax whitespace;

        public Any(string name, ISyntax whitespace)
        {
            this.Name = name;
            this.whitespace = whitespace;
            children = new List<IGrammar>();
        }
         
        public IGrammar Define(IEnumerable<IGrammar> grammars)
        {
            this.children = grammars.ToList();
            return this;
        }


        public bool Parse(ref Lexer lexer, out Node node)
        {
            whitespace.Parse(ref lexer); // consume whitespace.
            Node failure = null;

            node = new Node(this, lexer.Consume());
            var lexerbm = lexer;

            foreach (var grammar in children)
            {
                lexer.Reset(lexerbm);

                bool ok = grammar.Parse(ref lexer, out Node n);
                if (ok)
                {
                    node.Append(n);
                    return true;
                }
                else
                {
                    failure = Nodes.GetLongest(n, failure);
                }
            }

            node.Append(failure);
            return false;
                
        }

        public override string ToString()
        {
            return $"{Name}({nameof(Any)})";
        }

       
    }

}