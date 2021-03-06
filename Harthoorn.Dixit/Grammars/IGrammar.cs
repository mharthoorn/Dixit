using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public interface IGrammar
    {
        string Name { get; }
        bool Parse(ref Lexer lexer, out SyntaxNode node);

        IEnumerable<IGrammar> GetChildren();
    }
   
}