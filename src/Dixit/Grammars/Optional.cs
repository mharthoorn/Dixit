namespace Harthoorn.Dixit
{

    public class Optional : IGrammar
    {
        public string Name {get;}
        IGrammar grammar;

        public Optional(string name)
        {
            this.Name = name;
        }

        public Optional Define(IGrammar grammar)
        {
            this.grammar = grammar;
            return this;
        }

        public bool Parse(ref Lexer lexer, out Node node)
        {
            var bookmark = lexer;
            node = new Node(this);

            var ok = grammar.Parse(ref lexer, out Node n);
            node.Append(n); // always save good or bad.
            if (!ok) lexer.Reset(bookmark);
            return true;
        }
    }

}