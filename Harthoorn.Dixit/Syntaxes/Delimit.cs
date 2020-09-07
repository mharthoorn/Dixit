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
            bool esc = false;

            bool ok = lexer.Advance(Start);
            if (!ok) return lexer.Capture(false);
            
            do 
            {
                if (!esc && lexer.Current == End) break;
                esc = (lexer.Current == Escape);
                ok = lexer.AdvanceWhile();
            } 
            while (ok);
            ok = lexer.Advance(End);
            return lexer.Capture(ok);
        }

        public override string ToString()
        {
            return nameof(Delimiters);
        }
    }

    
   
}