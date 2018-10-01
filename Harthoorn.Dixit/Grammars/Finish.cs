namespace Harthoorn.Dixit
{
    public class Finish : IGrammar
    {
        public string Name { get; }
        public bool Parse(ref Lexer lexer, out Node node)
        {
            var token = lexer.Finish();
            node = Nodes.Create(this, null, token);
            return token.IsValid;
        }
    }
}
