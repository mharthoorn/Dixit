namespace Harthoorn.Dixit
{
    public class Compiler
    {
        Language language;

        public Compiler(Language language)
        {
            this.language = language;
        }

        public bool Parse(ISourceFile file, out Node root)
        {
            var grammar = language.Program;
            var lexer = new Lexer(file);
            bool success = grammar.Parse(ref lexer, out root);
            if (success) root.Prune();
            return success;
        }
    }
}