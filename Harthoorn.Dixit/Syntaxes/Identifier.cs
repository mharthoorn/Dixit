using System.Linq;

namespace Harthoorn.Dixit
{
    public class Identifier : ISyntax
    {
        char[] alpha;
        char[] numeric;
        char underscore = '_';

        public Identifier()
        {
            alpha = SyntaxHelper.CharArray("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz");
            numeric = SyntaxHelper.CharArray("0123456789");
        }
        public Token Parse(ref Lexer lexer)
        {
            bool valid = alpha.Contains(lexer.Current) || lexer.Current == underscore;

            if (valid)
            {
                lexer.Advance();
                while (alpha.Contains(lexer.Current) || numeric.Contains(lexer.Current) || lexer.Current == underscore)
                {
                    lexer.Advance();
                }
            }
            return lexer.Capture(valid);
        }
    }


}