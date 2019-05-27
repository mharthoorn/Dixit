namespace Harthoorn.Dixit
{
    public class EndOfString : ISyntax
    {
        public Token Parse(ref Lexer lexer)
        {
            while (lexer.Advance());
            return lexer.Capture(lexer.Consumable == 0);
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
            node = new SyntaxNode(this, token);
            return token.IsValid;

        }

        public override string ToString()
        {
            return Name;
        }
    }
}
