namespace Ace
{

    public class Optional : IGrammar
    {
        public string Name {get;}
        IGrammar grammar;

        public Optional(string name, IGrammar grammar)
        {
            this.Name = name;
            this.grammar = grammar;
        }

        public bool Parse(ref Lexer lexer, out Node node)
        {
            var bookmark = lexer;
            node = new Node(this, Token.Empty());

            var ok = grammar.Parse(ref lexer, out Node n);
            if (ok) // could be improved by: if we're too deep, the optional is probably the case, but just wrong. It's not misbranched
            {
                node.Append(n);
                
            }
            else 
            {
                lexer.Reset(bookmark);
            }
            return true;
        }
    }

}