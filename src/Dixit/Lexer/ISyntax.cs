
namespace Ace
{
    public interface ISyntax
    {
        Token Parse(ref Lexer lexer);
    }
    
   
}