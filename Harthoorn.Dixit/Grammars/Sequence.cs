using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{
    public class Sequence : IGrammar
    {
        public string Name { get; }
        public List<IGrammar> List { get; }
        ISyntax whitespace;

        public Sequence(string name, ISyntax whitespace, IEnumerable<IGrammar> items)
        {
            this.Name = name;
            this.whitespace = whitespace;
            this.List = items.ToList();
        }

        public bool Parse(ref Lexer lexer, out SyntaxNode node)
        {
            whitespace.Parse(ref lexer); // consume whitespace.
            node = new SyntaxNode(this, lexer.Here);

            foreach (var grammar in List)
            {
                whitespace.Parse(ref lexer);

                bool ok = grammar.Parse(ref lexer, out SyntaxNode n);
                node.Append(n);
                if (!ok) return false;
            }

            return true;
        }


        public override string ToString()
        {
            return $"{Name} ({nameof(Sequence)})";
        }

        public IEnumerable<IGrammar> GetChildren()
        {
            return List;
        }
    }

}