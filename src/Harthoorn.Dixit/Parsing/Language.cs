using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public class Language 
    {
        public IGrammar Program { get; private set; }
        public ISyntax WhiteSpace { get; set; }
        IGrammar RootGrammar { get; set; }
        IGrammar Finish { get; set; }
        List<IGrammar> rules;
        

        public Language()
        {
            rules = new List<IGrammar>();
        }

        public T Declare<T>(string name) where T : IGrammar, new()
        {
            var grammar = new T();
            rules.Add(grammar);
            return grammar;
        }

        public void Add(IGrammar grammar)
        {
            rules.Add(grammar);
        }

        public void Root(IGrammar root)
        {
            this.RootGrammar = root;
            this.Program = new Sequence("Program", WhiteSpace);
            var finish = new Finish();
            Program.Define(root, finish);
        }
    }
}