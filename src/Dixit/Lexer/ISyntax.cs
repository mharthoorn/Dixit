
namespace Harthoorn.Dixit
{
    public interface ISyntax
    {
        Token Parse(ref Lexer lexer);
    }
    
   
}