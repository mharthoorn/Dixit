
namespace Harthoorn.Dixit
{
    public interface IGrammar
    {
        string Name { get; }
        bool Parse(ref Lexer lexer, out Node node);
    }
    
   
}