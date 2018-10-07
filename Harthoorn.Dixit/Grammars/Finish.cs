namespace Harthoorn.Dixit
{
    public class EOF : IGrammar
    {
        public string Name { get; } = "EOF";

        public bool Parse(ref Lexer lexer, out Node node)
        {
            var token = lexer.Finish();
            node = Nodes.Create(this, null, token);
            return token.IsValid;
        }

        public bool ExpectingConcept { get; } = false;
    }
}
