namespace Harthoorn.Dixit
{
    public interface IGrammar
    {
        string Name { get; }
        bool Parse(ref Lexer lexer, out SyntaxNode node);
    }

    public static class IGrammarExtensions
    {
        public static void Fold(this IGrammar grammar, SyntaxNode node)
        {
            if (node.Token.IsValid)
                node.Grammar = grammar;
        }
    }
   
}