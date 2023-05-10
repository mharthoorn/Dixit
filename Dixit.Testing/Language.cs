using Harthoorn.Dixit;

namespace Dixit.Testing;

public static class Language 
{
    public static ISyntax
        keyword = new CharSet(minlength: 2, "abcdefghijklmnopqrstuvwxyz"),
        whitespace = new CharSet(0, ' ', '\n', '\r');
    
    public static IGrammar
        EOF = new EOF(whitespace);

    public static Concept
        Field = new Concept("field", whitespace),
        Fieldlist = new Concept("fieldlist", whitespace),
        Grammar = new Concept("language", whitespace);

    static Language()
    {
        Field.As(keyword); 
        Fieldlist.Interlace(",", Field, 2);
        Grammar.Sequence(Fieldlist, EOF);
    }

    public static bool Compile(string text, out SyntaxNode ast)
    {
        return Grammar.Compile(text, out ast);
    }
        
}