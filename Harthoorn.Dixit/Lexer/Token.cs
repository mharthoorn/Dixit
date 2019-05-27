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

        public Token(ISourceFile file, int start, int end, bool valid)
        {
            this.File = file;
            this.Start = start;
            this.End = end;
            this.IsValid = valid;
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