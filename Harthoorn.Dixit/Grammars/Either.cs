using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{
    public class Either : IGrammar
    {
        public string Name { get; }
        List<IGrammar> Children { get; set; }
        ISyntax whitespace;

        public Either(string name, ISyntax whitespace, IEnumerable<IGrammar> grammars)
        {
            this.Name = name;
            this.whitespace = whitespace;
            Children = grammars.ToList();
        }
         
        
        public bool Parse(ref Lexer lexer, out SyntaxNode node)
        {
            whitespace.Parse(ref lexer); // consume whitespace.
            var branches = new List<SyntaxNode>();

            node = new SyntaxNode(this, lexer.Here);
            var bookmark = lexer;
            bool ok = false;

            foreach (var grammar in Children)
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
                longest.Mark(State.Dismissed);
                node.Append(longest);
            }
            return ok;
                
        }

        public override string ToString()
        {
            return $"{Name} ({nameof(Either)})";
        }

       
    }

}