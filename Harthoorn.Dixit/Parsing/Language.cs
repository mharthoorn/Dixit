using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public class Language 
    {
        public Concept Grammar { get; private set; }
        public ISyntax WhiteSpace { get; set; }
        List<IGrammar> elements;
        

        public Language()
        {
            elements = new List<IGrammar>();
        }

        public void Add(IGrammar grammar)
        {
            elements.Add(grammar);
        }

        public void DefineGrammar(IGrammar program)
        {
            this.Grammar = new Concept(this, "Grammar");
            var eof = new EOF();
            Grammar.Sequence(program, eof);

        }
    }
}