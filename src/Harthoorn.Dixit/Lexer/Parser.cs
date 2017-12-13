using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public class Parser
    {
        public static Token? TryParse(ISyntax syntax, ref Lexer lexer)
        {
            return syntax.Parse(ref lexer);
        }

        public static IEnumerable<Token> Parse(ISyntax syntax, Lexer lexer)
        {
            TryParse(syntax, ref lexer);
            return null;
        }

    }
}