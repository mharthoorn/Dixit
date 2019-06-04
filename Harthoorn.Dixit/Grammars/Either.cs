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

            foreach (var grammar in Children)
            {
                lexer = bookmark;

                bool ok = grammar.Parse(ref lexer, out SyntaxNode n);

                if (ok)
                {
                    node.Append(n);
                    return true;
                }
                else
                {
                    branches.Add(n);
                }
            }

            var longest = branches.GetFarthest();
            longest.MarkAsError();
            node.Append(longest);
            return false;
                
        }

        public override string ToString()
        {
            return $"{Name} ({nameof(Either)})";
        }

       
    }

}