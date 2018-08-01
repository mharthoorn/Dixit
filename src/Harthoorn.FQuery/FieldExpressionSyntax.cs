using Harthoorn.Dixit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harthoorn.FQuery
{
    class FieldExpressionSyntax : ISyntax
    {
        static readonly string DELIMITERS = ",\r\n ";

        public Token Parse(ref Lexer lexer)
        {
            bool inside = true;
            bool literal = false;
            // this will need some improvement!
            while (inside && lexer.Advance(1))
            {
                var c = lexer.Current;
                if (c == '\'') literal = !literal;
                inside = literal || !c.In(DELIMITERS);
            }
            return lexer.Consume().FailsOnEmpty();
        }
    }

    public static class CharExtensions
    {
        public static bool In(this char c, string set)
        {
            return set.IndexOf(c) >= 0;
        }
    }
}
