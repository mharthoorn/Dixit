using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public class Syntax : IGrammar
    {
        public string Name { get; }
        ISyntax syntax;

        public Syntax(string name, ISyntax syntax)
        {
            this.Name = name;
            this.syntax = syntax;
        }

        public bool Parse(ref Lexer lexer, out SyntaxNode node)
        {
            var token = syntax.Parse(ref lexer);
            node = Nodes.Create(this, syntax, token);
            return token.IsValid;
        }

        public override string ToString()
        {
            return $"{Name} ({syntax.GetType().Name})";
        }

        public IEnumerable<IGrammar> GetChildren()
        {
            yield break; // no child grammars
        }
    }
}
