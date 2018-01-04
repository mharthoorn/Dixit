using Harthoorn.Dixit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harthoorn.FQuery
{
    class FieldExpressionSyntax : ISyntax
    {
        public Token Parse(ref Lexer lexer)
        {
            // this will need some improvement!

            lexer.Advance(c => c != ',' && c != '\r' && c != '\n'); 
            return lexer.Consume().FailsOnEmpty();
        }
    }
}
