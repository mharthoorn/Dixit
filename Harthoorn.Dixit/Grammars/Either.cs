using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{
    public class Either : IGrammar
    {
        public string Name { get; }
        IList<IGrammar> Children { get; set; }
        ISyntax whitespace;

        public Either(string name, ISyntax whitespace, IEnumerable<IGrammar> grammars)
        {
            this.Name = name;
            this.whitespace = whitespace;
            Children = grammars.ToList();
        }
         
        
        public bool Parse(ref Lexer lexer, out Node node)
        {
            whitespace.Parse(ref lexer); // consume whitespace.
            Node failure = null;

            node = new Node(this, lexer.Consume());
            var bookmark = lexer;

            foreach (var grammar in Children)
            {
                lexer.Reset(bookmark);

                bool ok = grammar.Parse(ref lexer, out Node n);
                if (ok)
                {
                    node.Append(n, dismiss: true);
                    return true;
                }
                else
                {
                    failure = Nodes.GetLongest(n, failure);
                }
            }

            failure.State = State.Error;
            node.Append(failure);
            return false;
                
        }

        public override string ToString()
        {
            return $"{Name} ({nameof(Either)})";
        }

       
    }

}