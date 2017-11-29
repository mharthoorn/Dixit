using System.Linq;

namespace Ace
{
    public class CharSet : ISyntax
    {
        private readonly int minlength;
        char[] characters;

        public CharSet(int minlength, params char[] chars)
        {
            this.minlength = minlength;
            this.characters = chars;
        }

        public CharSet(int minlength, params string[] chars)
        {
            this.minlength = minlength;
            this.characters = chars.SelectMany(s => s.ToCharArray()).ToArray();
        }

        public Token Parse(ref Lexer lexer)
        {
            int i = lexer.Advance(characters.Contains);
            return lexer.Consume().FailWhen(i < minlength);
        }

        public override string ToString()
        {
            string charstring = new string(characters);
            return $"{nameof(CharSet)}-{charstring}";
        }
    }

    
   
}