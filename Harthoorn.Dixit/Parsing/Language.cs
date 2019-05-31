using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public class Language 
    {
        public Concept Grammar;
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
            Grammar = new Concept("Grammar", this);
            var eof = new EOF(WhiteSpace);
            Grammar.Sequence(program, eof);

        }
    }

    
}