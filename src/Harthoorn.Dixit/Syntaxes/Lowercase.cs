namespace Harthoorn.Dixit
{
    public class Lowercase : ISyntax
    {

        public Token Parse(ref Lexer lexer)
        {
            lexer.Advance(c => c >= 'a' && c <= 'z');
            return lexer.Consume().FailsOnEmpty();
        }

        public override string ToString()
        {
            return $"{nameof(Lowercase)}";
        }
    }

    
   
}