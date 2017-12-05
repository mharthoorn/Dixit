using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit.Sql
{
   
   public class SqlCompiler 
   {
        Language language;

        public SqlCompiler()
        {
            language = DefineLanguage();
        }

        ISyntax
            keyword;

        IGrammar
            whitespace, star, fieldname, comma, stringvalue,
            field, equalityoperator, fieldlist, fromclause, equality_expression,
            boolean_operator, boolean_expression, brackets_expression, expression,
            whereclause, statement, optionalwhereclause;


        public Language DefineLanguage()
        {
            var language = new Language();
            
            whitespace = language.WhiteSpace(' ', '\n', '\t');
            keyword = new CharSet(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz");

            star = language.Literal("*");
            fieldname = language.CharSet("FieldName", 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz", "_");
            
            comma = language.Literal(",");
            field = language.Any("Field");
            
            stringvalue = language.Delimit("string", '"', '"', '\\');
            equalityoperator = language.Any("EqualityOperator");
            fieldlist = language.Interlace("FieldList", ",");
            fromclause = language.Sequence("FromClause");
            equality_expression = language.Sequence("Equation");
            boolean_operator =  language.Any("BooleanOperator");
            boolean_expression = language.Sequence("BooleanExpression");
            brackets_expression = language.Sequence("BracketExpressions");
            expression = language.Any("Expression");
            whereclause = language.Sequence("WhereClause");
            statement = language.Sequence("SelectStatement");

            optionalwhereclause = language.Optional("OptionalWhereClause");

            field
                .Define(fieldname, star);

            equalityoperator
                .Define("=", "!=", "<", ">");

            fieldlist
                .Define(field);

            fromclause
                .Define("from", fieldname);

            equality_expression
                .Define(fieldname, equalityoperator, stringvalue);

            boolean_operator
                .Define("and", "or");

            boolean_expression
                .Define(equality_expression, boolean_operator, expression);

            brackets_expression
                .Define("(", expression, ")");

            expression
                .Define(boolean_expression, equality_expression, brackets_expression);

            whereclause
                .Define("where", expression);

            optionalwhereclause
                .Define(whereclause);

            statement
                .Define("select", fieldlist, "from", fromclause, whereclause);

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