using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public class Language 
    {
        internal ISyntax WhiteSpace { get; set; }
        public IGrammar Root { get; set; }
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

        public (Node root, bool success) Parse(ISourceFile file)
        {
            var lexer = new Lexer(file);
            bool success = Root.Parse(ref lexer, out Node root);
            return (root, success);
        }
    }
    
}