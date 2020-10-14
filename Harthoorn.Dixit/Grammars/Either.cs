using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{
    public class Either : IGrammar
    {
        public string Name { get; }
        List<IGrammar> children;
        ISyntax whitespace;

        public Either(string name, ISyntax whitespace, IEnumerable<IGrammar> grammars)
        {
            this.Name = name;
            this.whitespace = whitespace;
            children = grammars.ToList();
        }
         
        
        public bool Parse(ref Lexer lexer, out SyntaxNode node)
        {
            whitespace.Parse(ref lexer); // consume whitespace.
            var branches = new List<SyntaxNode>();

            node = new SyntaxNode(this, lexer.Here);
            var bookmark = lexer;
            bool ok = false;

            foreach (var grammar in children)
            {
                lexer = bookmark;

                ok = grammar.Parse(ref lexer, out SyntaxNode n);

                if (ok)
                {
                    node.Append(n);
                    break;
                }
                else
                {
                    branches.Add(n);
                }
            }

            if (branches.Count > 0)
            {
                var longest = branches.GetFarthestEnd();
                if (ok)
                {
                    longest.Mark(State.Dismissed);
                }
                else
                {
                    longest.Mark(State.Error);
                }
                
                node.Append(longest);
            }

            return ok;
                
        }

        public override string ToString()
        {
            return $"{Name} ({nameof(Either)})";
        }

        public IEnumerable<IGrammar> GetChildren()
        {
            return this.children;
        }
    }

}