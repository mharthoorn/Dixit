using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit.Sql
{
   
   public class SqlCompiler 
   {
        Language language;

        ISyntax
            keyword;

        IGrammar
            whitespace, star, fieldname, comma, stringvalue,
            field, fieldlist, fromclause, eqalityoperator, equality_expression, 
            whereclause, optionalwhereclause, statement,
            expression, brackets_expression, boolean_expression, boolean_operator;
            
        public SqlCompiler()
        {
            language = DefineLanguage();
        }

        public Language DefineLanguage()
        {
            var language = new Language();
            whitespace = language.WhiteSpace(' ', '\n', '\t');

            star = language.Literal("*");
            fieldname = language.CharSet("FieldName", 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz", "_");
            keyword = new CharSet(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz");
            comma = language.Literal(",");
            field = language.Any("Field", fieldname, star);
            fieldlist = language.GluedSequence("FieldList", ",", field);
            fromclause = language.Sequence("FromClause", fieldname);
            stringvalue = language.Delimit("string", '"', '"', '\\');
            eqalityoperator = language.Any("EqualityOperator", "=", "!=", "<", ">");
            
            equality_expression = language.Sequence("Equation", fieldname, eqalityoperator, stringvalue);
            boolean_operator =  language.Any("BooleanOperator", "and", "or");
            boolean_expression = language.Sequence("BooleanExpression", equality_expression, boolean_operator, expression);
            brackets_expression = language.Sequence("BracketExpressions", "(", expression, ")");
            expression = language.Any("Expression", boolean_expression, equality_expression, brackets_expression);
            whereclause = language.Sequence("WhereClause", "where", expression);
            
            optionalwhereclause = language.Optional("OptionalWhereClause", whereclause);

            statement = language.Sequence("SelectStatement", "select", fieldlist, "from", fromclause, optionalwhereclause);

            language.Root = statement;
            return language;
        }

        public (Node, bool) Compile(string text)
        {
            var file = new SourceFile(text);
            return language.Parse(file);
        }

        public Query GetQuery(Node node)
        {
            var nodes = node.Descend(whereclause, boolean_expression, equality_expression).ToList();
            var dict = nodes.ToFilters();

            return new Query
            {
                Fields = node.Descend(statement, fieldlist, field, fieldname).Values().ToList(),
                Resource = node.Descend(fromclause, fieldname).Values().FirstOrDefault(),
                Where = node.Descend(whereclause, boolean_expression, equality_expression).ToFilters().ToList()
            };
        }

    }

    public class Query
    {
        public List<string> Fields;
        public string Resource;
        public List<Filter> Where;
    }

    public struct Filter
    {
        public string Name;
        public string Operator;
        public string Value;
    }

    public static class Extensions
    {
        public static IEnumerable<Filter> ToFilters(this IEnumerable<Node> range)
        {
            foreach (var node in range)
            {
                var filter = new Filter 
                {
                    Name = node.Children[0].Token.Text,
                    Operator = node.Children[1].Children[0].Token.Text,
                    Value = node.Children[2].Token.Text
                };
                yield return filter;
                
            }
        }

    }

}