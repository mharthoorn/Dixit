using System.Collections.Generic;

namespace Harthoorn.Dixit
{

    public class Concept : IGrammar
    {
        public string Name { get; }
        
        public IGrammar Grammar { get; internal set; }
        public ISyntax Whitespace { get; private set; }
         
        public Concept(string name, ISyntax whitespace) 
        {
            this.Name = name;
            Whitespace = whitespace;
        }

        public bool Parse(ref Lexer lexer, out SyntaxNode node)
        {
            bool ok = Grammar.Parse(ref lexer, out node);
            node.RebaseTo(this);

            return ok;
        }

        public override string ToString()
        {
            return $"{Name} (Concept)";
        }

        public IEnumerable<IGrammar> GetChildren()
        {
            yield return Grammar;
        }
    }
}