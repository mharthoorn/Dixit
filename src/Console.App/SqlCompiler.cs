using System.Collections.Generic;
using System.Linq;
using Ace;

namespace Ace.Sql
{
   
   public class SqlCompiler 
   {
        Language language;

        IGrammar
            whitespace, star, fieldname, comma, stringvalue,
            field, fieldlist, fromclause, eqalityoperator, equation, 
            whereclause, optionalwhereclause, statement;
            
        public SqlCompiler()
        {
            language = DefineLanguage();
        }

        public Language DefineLanguage()
        {
            var language = new Language();

            whitespace = language.SetWhitespace(' ', '\n', '\t');
            
            star = language.Literal("*");
            fieldname = language.CharSet("FIELD-NAME", 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz", "_");
            comma = language.Literal(",");
            field = language.Any("FIELD", fieldname, star);
            fieldlist = language.GluedSequence("FIELD-LIST", ",", field);
            fromclause = language.Sequence("FROM-CLAUSE", fieldname);
            stringvalue = language.Delimit("string", '"', '"', '\\');
            eqalityoperator = language.Any("EqOp", "=", "!=", "<", ">");
            equation = language.Sequence("EQUATION", fieldname, eqalityoperator, stringvalue);

            whereclause = language.Sequence("WHERE-CLAUSE", "where", equation);
            optionalwhereclause = language.Optional("OPTIONAL-WHERE-CLAUSE", whereclause);

            statement = language.Sequence("SELECT-STATEMENT", "select", fieldlist, "from", fromclause, optionalwhereclause);

            language.Root = statement;
            return language;
        }

        public (Node, bool) Compile(string text)
        {
            var file = new SourceFile(text);
            var compiler = new Compiler(language);
            return compiler.Compile(file);
        }

        public Query GetQuery(Node node)
        {
            var nodes = node.Descend(whereclause, equation).ToList();
            var tuples = nodes.Tuple(fieldname, stringvalue);
            var dict = tuples.ToDictionary();

            return new Query
            {
                Fields = node.Descend(statement, fieldlist, field, fieldname).Values().ToList(),
                Resource = node.Descend(fromclause, fieldname).Values().FirstOrDefault(),
                
                Where = node.Descend(whereclause, equation).Tuple(fieldname, stringvalue).ToDictionary()
            };
        }

    }

    public class Query
    {
        public List<string> Fields;
        public string Resource;
        public Dictionary<string, string> Where;
    }


    
   
}