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

        public void Add(IGrammar grammar)
        {
            rules.Add(grammar);
        }

        public (Node root, bool success) Parse(SourceFile file)
        {
            var lexer = new Lexer(file);
            bool success = Root.Parse(ref lexer, out Node root);
            return (root, success);
        }
    }
}