using System;
using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{
    public static class Grammar
    {
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

        public static IEnumerable<IGrammar> Grammarize(this object[] words)
        {
            return words.Select(Grammar.Grammarize).Where(g => g != null);
        }

        public static IGrammar Define(this IGrammar grammar, params object[] items)
        {
            var list = Grammarize(items);
            switch (grammar)
            {
                case Any g: g.Define(list); return g;
                case Sequence g: g.Define(list); return g;
                case Optional g: g.Define(list.FirstOrDefault()); return g;
                case Interlace g: g.Define(list.FirstOrDefault()); return g;
                default: throw new ArgumentException("Invalid define");
            }
        }

        public static Any Any(this Language language, string name)
        {
            var sequence = new Any(name, language.WhiteSpace);
            language.Add(sequence);
            return sequence;
        }

        public static Sequence Sequence(this Language language, string name)
        {
            var sequence = new Sequence(name, language.WhiteSpace);
            language.Add(sequence);
            return sequence;
        }

        public static Sequence Define(this Sequence sequence, params object[] words)
        {
            var list = words.Select(Grammar.Grammarize).Where(g => g != null);
            sequence.Define(list);
            return sequence;
        }

        public static IGrammar WhiteSpace(this Language language, params char[] characters)
        {
            var syntax = new CharSet(0, characters);
            language.WhiteSpace = syntax; 
            var grammar = Grammar.Grammarize(syntax);
            language.Add(grammar);
            return grammar;
        }

        public static Interlace Interlace(this Language language, string name, ISyntax glue)
        {
            var sequence = new Interlace(name, glue, language.WhiteSpace);
            language.Add(sequence);
            return sequence;
        }

        public static Interlace Interlace(this Language language, string name, string glue)
        {
            var glueSyntax = new Literal(glue);
            var sequence = new Interlace(name, glueSyntax, language.WhiteSpace);
            language.Add(sequence);
            return sequence;
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

        public static Optional Optional(this Language language, string name)
        {
            var optional = new Optional(name);
            language.Add(optional);
            return optional;
        }
                
    }
}