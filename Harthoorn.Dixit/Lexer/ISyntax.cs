
namespace Harthoorn.Dixit
{
    /// <summary>
    /// Parses a token, and advances the Lexer cursor when succesful.
    /// </summary>
    public interface ISyntax
    {
        Token Parse(ref Lexer lexer);
    }
    
   
}