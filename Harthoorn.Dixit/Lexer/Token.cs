using System;

namespace Harthoorn.Dixit
{
    public struct Token
    {
        public ISourceFile File;
        public int Start;
        public int End;

        public int Length => End - Start;
        public string Text => File.Span(Start, End);
        
        public bool IsValid;
        // no error message. Token is too bare metal for that.

        public Token(ISourceFile file, int start, int end)
        {
            this.File = file;
            this.Start = start;
            this.End = end;
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

        public bool IsEmpty => this.Length == 0;


        public void ExpandWith(Token token)
        {
            if (token.End > this.End) this.End = token.End;
        }

        public override string ToString()
        {
            string text = Text.Replace("\r", "{\\r}").Replace("\n", "{\\n}") ;
            return $"\"{text}\" [{Start}..{End}]";
        }
    }

}