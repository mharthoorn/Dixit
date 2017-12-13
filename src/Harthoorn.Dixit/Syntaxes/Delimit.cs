namespace Harthoorn.Dixit
{
    public class Delimiters : ISyntax
    {
        public char Start;
        public char Escape;
        public char End;

        public Delimiters(char start, char end, char esc)
        {
            this.Start = start;
            this.End = end;
            this.Escape = esc;
        }

        public Token Parse(ref Lexer lexer)
        {
            bool ok = true, esc = false;
            var bookmark = lexer;
            lexer.Advance(Start);
            lexer.Consume();
            do 
            {
                if (!esc && lexer.Current == End) break;
                esc = (lexer.Current == Escape);
                ok = lexer.Advance();
            } 
            while (ok);
            var token = lexer.Consume();
            if (ok) { lexer.Advance(); lexer.Consume(); } // consume the End character.
            //token.Text = token.Text.TrimStart(Start).TrimEnd(End);
            return token.FailWhen(!ok);
        }

        public override string ToString()
        {
            return nameof(Delimiters);
        }
    }

    
   
}