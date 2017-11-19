using System;

namespace Ace
{
    public struct Token
    {
        // will be a span
        public string Text;
        public int Start;
        public int Length;
        
        public bool IsValid;
        // no error message. Token is too bare metal for that.

        public Token(string text, int start, int length)
        {
            Text = text;
            this.Start = start;
            this.Length = length;
            this.IsValid = true;
        }

        public Token FailsOnEmpty()
        {
            return FailWhen(IsEmpty);
        }

        public Token FailWhen(Predicate<Token> predicate)
        {
            if (predicate(this))
            {
                this.IsValid = false;
            }
            return this;
        }

         public Token FailWhen(bool predicate)
        {
            if (predicate)
            {
                this.IsValid = false;
            }
            return this;
        }

        public static bool IsEmpty(Token token)
        {
            return token.Length == 0;
        }

        public static Token Empty()
        {
            return new Token("", 0, 0);
        }

        public override string ToString()
        {
            return $"\"{Text}\" ({Start},{Length})";
        }
    }

}