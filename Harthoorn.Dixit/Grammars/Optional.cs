namespace Harthoorn.Dixit
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
            node = new Node(this, lexer.Here);

            var ok = grammar.Parse(ref lexer, out Node n);
            node.Append(n, dismiss: true); // always save good or bad.
            if (!ok) lexer.Reset(bookmark);
            return true; 
        }

        public override string ToString()
        {
            return nameof(Optional);
        }
    }

}