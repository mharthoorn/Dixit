using System.Text.RegularExpressions;

namespace Harthoorn.Dixit;

public class Keyword : ISyntax
{
    string keyword;
    Regex regex = new(@"\G\w+");

    public Keyword(string keyword)
    {
        this.keyword = keyword;
    }

    public Token Parse(ref Lexer lexer)
    {
        var ok = lexer.Advance(regex);
        var token = lexer.Capture(ok);
        
        if (token.Text != keyword) token.IsValid = false;
        
        return token;
    }
}

