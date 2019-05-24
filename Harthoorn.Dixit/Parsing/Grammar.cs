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
                case string s: return new Syntax(s, new Literal(s));
                default: return null;
            }
        }

        public static IEnumerable<IGrammar> Grammarize(this object[] words)
        {
            return words.Select(Grammar.Grammarize).Where(g => g != null);
        }

        //public static IGrammar Range(this IGrammar grammar, params object[] items)
        //{
        //    if (grammar is null) throw new NullReferenceException("Define failed. Grammar is null");

        //    var list = Grammarize(items);
        //    switch (grammar)
        //    {
        //        case Any g: g.Define(list); return g;
        //        case Sequence g: g.Define(list); return g;
        //        case Optional g: g.Define(list.FirstOrDefault()); return g;
        //        case Interlace g: g.Define(list.FirstOrDefault()); return g;
        //        default: throw new ArgumentException("Invalid define");
        //    }
        //}


        public static Any Any(this Concept concept, params object[] words)
        {
            var grammars = Grammarize(words);
            var any = new Any(concept.Name+"-any", concept.Language.WhiteSpace);
            any.Define(grammars);
            concept.Grammar = any;
            return any;
        }

        public static Sequence Sequence(this Concept concept, params object[] words)
        {
            var grammars = Grammarize(words);
            var sequence = new Sequence(concept.Name, concept.Language.WhiteSpace, grammars);
            return sequence;
        }

        public static IGrammar Syntax(this Language language, string name, ISyntax syntax)
        {
            var grammar = new Syntax(name, syntax);
            language.Add(grammar);
            return grammar;
        }

        public static IGrammar WhiteSpace(this Language language, params char[] characters)
        {
            var syntax = new CharSet(0, characters);
            language.WhiteSpace = syntax; 
            var grammar = Grammar.Grammarize(syntax);
            language.Add(grammar);
            return grammar;
        }

        public static void Interlace(this Concept concept, string glue, IGrammar grammar)
        {
            var glueSyntax = new Literal(glue);
            var interlace = new Interlace(concept.Name+"-interlace", glueSyntax, concept.Language.WhiteSpace);
            concept.Grammar = interlace;
        }

        public static IGrammar Literal(this Language language, string name, string literal)
        {
            var syntax = new Literal(literal);
            var grammar = new Syntax(name, syntax);
            language.Add(grammar);
            return grammar;
        }

        public static void As(this Concept concept, IGrammar grammar)
        {
            concept.Grammar = grammar;
        }

        public static Concept As(this Concept concept, ISyntax syntax)
        {
            var grammar = new Syntax(concept.Name + "-syntax", syntax);
            concept.Grammar = grammar;
            return concept;
        }


        public static Concept Define(this Language language, string name)
        {
            var grammar = new Concept(language, name);
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

        public static Concept Delimit(this Concept concept, string name, char start, char end, char esc)
        {
            var syntax = new Delimiters(start, end, esc);
            var grammar = new Syntax(name, syntax);
            concept.Grammar = grammar;
            return concept;
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

        public static Concept Optional(this Concept concept, IGrammar grammar)
        {
            var optional = new Optional($"Optional({concept.Name})", grammar);
            concept.Grammar = optional;
            return concept;
        }
                

    }
}