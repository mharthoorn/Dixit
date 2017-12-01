using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{
    public class Compiler
    {
        Language language;

        public Compiler(Language language)
        {
            this.language = language;
        }

        public (Node root, bool success) Compile(SourceFile file)
        {
            var lexer = new Lexer(file);
            bool success = language.Root.Parse(ref lexer, out Node root);
            return (root, success);
        }
    }

   
}