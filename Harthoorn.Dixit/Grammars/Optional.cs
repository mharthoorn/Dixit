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
        
        public bool Parse(ref Lexer lexer, out SyntaxNode node)
        {
            var bookmark = lexer;
            node = new SyntaxNode(this, lexer.Here);

            var ok = grammar.Parse(ref lexer, out SyntaxNode n);
            if (!ok) lexer = bookmark;
            node.Append(n, errorproliferation: State.Dismissed); // always save good or bad.
            
            return true; 
        }

        public override string ToString()
        {
            return Name;
        }
    }

}