using System.Linq;

namespace Harthoorn.Dixit
{
    public class Literal : ISyntax
    {
        private string literal;
        private bool ignoreCase;

        public Literal(string literal, bool ignoreCase = false)
        {
            this.literal = literal;
            this.ignoreCase = ignoreCase;
        }

        public Token Parse(ref Lexer lexer)
        {
            var ok = lexer.Advance(literal, ignoreCase);
            return lexer.Consume().FailWhen(!ok);
        }

        public override string ToString()
        {
            return $"{literal}";
        }

    }


}