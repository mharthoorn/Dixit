namespace Harthoorn.Dixit
{
    public class Lowercase : ISyntax
    {

        public Token Parse(ref Lexer lexer)
        {
            lexer.AdvanceWhile(c => c >= 'a' && c <= 'z');
            return lexer.Capture(lexer.Consumable > 0);

        }

        public override string ToString()
        {
            return $"{nameof(Lowercase)}";
        }
    }

    
   
}