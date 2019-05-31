namespace Harthoorn.Dixit
{
    public static class Compiler
    {
        public static bool Compile(this IGrammar grammar, ISourceFile file, out SyntaxNode ast)
        { 
            var lexer = new Lexer(file);
            bool success = grammar.Parse(ref lexer, out ast);
            ast.Prune();
            return success;
        }

        public static bool Compile(this IGrammar grammar, string text, out SyntaxNode ast)
        {
            var file = new MemoryFile(text);
            var lexer = new Lexer(file);
            bool success = grammar.Parse(ref lexer, out ast);
            ast.Prune();
            return success;
        }
    }
}