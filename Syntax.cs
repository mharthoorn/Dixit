using System.Collections.Generic;
using System.Linq;

namespace Ace
{
    public interface ISyntax
    {
        Token Parse(ref Lexer lexer);
    }

    public class Literal : ISyntax
    {
        private string literal;

        public Literal(string literal)
        {
            this.literal = literal;
        }

        public Token Parse(ref Lexer lexer)
        {
            var ok = lexer.Advance(literal);
            return lexer.Consume().FailWhen(!ok);
        }

        public override string ToString()
        {
            return $"{literal}";
        }

    }

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

    public class CharSet : ISyntax
    {
        public string Name { get; }
        private readonly int minlength;
        char[] characters;

        public CharSet(string name, int minlength, params char[] chars)
        {
            this.Name = name;
            this.minlength = minlength;
            this.characters = chars;
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