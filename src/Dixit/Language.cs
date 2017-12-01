using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{
    public class Language 
    {
        public ISyntax WhiteSpace;
        List<IGrammar> rules;
        public IGrammar Root;

        public Language()
        {
            rules = new List<IGrammar>();
        }

        public void Add(IGrammar grammar)
        {
            rules.Add(grammar);
        }

        public void SetWhitespace(ISyntax whitespace)
        {
            this.WhiteSpace = whitespace;

        }
        
        public static IGrammar Grammarize(object word)
        {
            switch (word)
            {
                case IGrammar g: return g;
                case ISyntax syntax: return new Syntax(syntax.GetType().Name, syntax);
                case string s: return new Syntax("Literal", new Literal(s));
                default: return null;
            }
        }

    }

    public static class LanguageExtensions
    {
        public static IGrammar Any(this Language language, string name, params object[] words)
        {
            var list = words.Select(Language.Grammarize).Where(g => g != null).ToList();
            var sequence = new Any(name, list, language.WhiteSpace);
            language.Add(sequence);
            return sequence;
        }

        public static IGrammar Sequence(this Language language, string name, params object[] words)
        {
            var list = words.Select(Language.Grammarize).Where(g => g != null).ToList();
            var sequence = new Sequence(name, list, language.WhiteSpace);
            language.Add(sequence);
            return sequence;
        }

        public static IGrammar GluedSequence(this Language language, string name, ISyntax glue, IGrammar item)
        {
            var sequence = new GluedSequence(name, item, glue, language.WhiteSpace);
            language.Add(sequence);
            return sequence;
        }

        public static IGrammar GluedSequence(this Language language, string name, string glue, IGrammar item)
        {
            var glueSyntax = new Literal(glue);
            var sequence = new GluedSequence(name, item, glueSyntax, language.WhiteSpace);
            language.Add(sequence);
            return sequence;
        }

        public static IGrammar SetWhitespace(this Language language, params char[] characters)
        {
             var syntax = new CharSet(0, characters);
             language.SetWhitespace(syntax);
             var grammar = Language.Grammarize(syntax);
             language.Add(grammar);
             return grammar;
        }

        public static IGrammar Literal(this Language language, string literal)
        {
            var syntax = new Literal(literal);
            var grammar = new Syntax(literal, syntax);
            language.Add(grammar);
            return grammar;
        }

        public static IGrammar CiLiteral(this Language language, string literal)
        {
            var syntax = new Literal(literal, ignoreCase: true);
            var grammar = new Syntax(literal, syntax);
            language.Add(grammar);
            return grammar;
        }

        public static IGrammar Delimit(this Language language, string name, char start, char end, char esc)
        {
            var syntax = new Delimit(start, end, esc);
            var grammar = new Syntax(name, syntax);
            language.Add(grammar);
            return grammar;
        }

        public static IGrammar CharSet(this Language language, string name, params string[] chars)
        {
            return language.CharSet(name, 0, chars); 
        }

        public static IGrammar CharSet(this Language language, string name, int minlength, params string[] chars)
        {
            var charset = new CharSet(minlength, chars);
            var unit = new Syntax(name, charset);
            language.Add(unit);
            return unit;
        }

        public static IGrammar Optional(this Language language, string name, IGrammar grammar)
        {
            var optional = new Optional(name, grammar);
            language.Add(optional);
            return optional;
        }
        
    }
}