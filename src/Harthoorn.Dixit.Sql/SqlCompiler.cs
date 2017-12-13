using System;
using System.Linq;

namespace Harthoorn.Dixit.Sql
{

    public class SqlCompiler
    {

        public (Node, bool) Compile(string text)
        {
            var file = new MemoryFile(text);
            var compiler = new Compiler(SQL.Language);
            var success = compiler.Parse(file, out Node root);
            return (root, success);
        }

        public Query GetQuery(Node node)
        {
            var nodes = node.Descend(SQL.WhereClause, SQL.BooleanExpression, SQL.EqualityExpression).ToList();
            var dict = nodes.ToFilters();

            return new Query
            {
                Fields = node.Descend(SQL.Statement, SQL.FieldList, SQL.Field, SQL.FieldName).Values().ToList(),
                Resource = node.Descend(SQL.FromClause, SQL.FieldName).Values().FirstOrDefault(),
                Where = node.Descend(SQL.WhereClause, SQL.BooleanExpression, SQL.EqualityExpression).ToFilters().ToList()
            };
        }

    }

}