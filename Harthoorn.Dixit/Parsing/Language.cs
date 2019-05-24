using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public class Language 
    {
        public Concept Grammar { get; private set; }
        public ISyntax WhiteSpace { get; set; }
        List<IGrammar> concepts;
        

        public Language()
        {
            concepts = new List<IGrammar>();
        }

        public void Add(IGrammar grammar)
        {
            concepts.Add(grammar);
        }

        public void DefineGrammar(IGrammar program)
        {
            this.Grammar = new Concept(this, "Compilation");
            var eof = new EOF();
            Grammar.Sequence(program, eof);

        }
    }
}