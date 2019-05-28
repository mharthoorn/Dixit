namespace Harthoorn.Dixit
{
    public class EndOfString : ISyntax
    {
        public Token Parse(ref Lexer lexer)
        {
            return lexer.Capture(lexer.IsAtEnd);
        }
    }

    public class EOF : IGrammar
    {
        public string Name { get; } = "EOF";

        ISyntax whitespace;
        ISyntax eof;

        public EOF(ISyntax whitespace)
        {
            this.whitespace = whitespace;
            this.eof = new EndOfString();
        }

        public bool Parse(ref Lexer lexer, out SyntaxNode node)
        {
            whitespace.Parse(ref lexer);
            var token = eof.Parse(ref lexer);
            var state = (token.IsValid) ? State.Valid : State.Error;
            node = new SyntaxNode(this, eof, token, state);
            return token.IsValid;

        }

        public override string ToString()
        {
            return Name;
        }
    }
}
