using System.Collections.Generic;
using Ace;

namespace Ace.Sql
{
   
   public class SqlCompiler 
   {
        Language language;
      
        public SqlCompiler()
        {
            language = DefineLanguage();
        }

        public Language DefineLanguage()
        {
            var language = new Language();

            var whitespace = language.SetWhitespace(' ', '\n', '\t');
            
            var star = language.Literal("*");
            var identifier = language.CharSet("FIELD-NAME", 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz", "_");
            var comma = new Literal(",");
            var field = language.Any("FIELD", identifier, star);
            var fieldlist = language.GluedSequence("FIELD-LIST", ",", field);
            var fromclause = language.Sequence("FROM-CLAUSE", identifier);
            var expression = language.Literal("A");
            var equalityop = language.Literal("=");
            var equation = language.Sequence("EQUATION", expression, equalityop, expression);

            var whereclause = language.Sequence("WHERE-CLAUSE", "where", equation);
            var optionalwhereclause = language.Optional("OPTIONAL-WHERE-CLAUSE", whereclause);

            var statement = language.Sequence("SELECT-STATEMENT", "select", fieldlist, "from", fromclause, optionalwhereclause);
            
            

            language.Root = statement;
            return language;
        }

        public (Node, bool) Compile(string text)
        {
            var file = new SourceFile(text);
            var compiler = new Compiler(language);
            return compiler.Compile(file);
        }

    }

    public class Keyword : ISymbol
    {

    }

    public class Field : ISymbol
    {

    }

    public class FieldList : ISymbol
    {

    }


    
   
}