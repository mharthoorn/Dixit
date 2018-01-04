using System;
using System.Linq;
using Harthoorn.Dixit;
 
namespace Harthoorn.FQuery
{

    public class FQueryCompiler
    {

        public (Node, bool) Compile(string text)
        {
            var file = new MemoryFile(text);
            var compiler = new Compiler(FQL.Language);
            var success = compiler.Parse(file, out Node root);
            return (root, success);
        }

        public Query GetQuery(Node node)
        {
            var nodes = node.Descend(FQL.WhereClause, FQL.BooleanExpression, FQL.EqualityExpression).ToList();
            var dict = nodes.ToFilters();

            return new Query
            {
                Fields = node.Descend(FQL.Statement, FQL.FieldList, FQL.Field, FQL.FieldName).Values().ToList(),
                Resource = node.Descend(FQL.FromClause, FQL.FieldName).Values().FirstOrDefault(),
                Where = node.Descend(FQL.WhereClause, FQL.BooleanExpression, FQL.EqualityExpression).ToFilters().ToList()
            };
        }

    }

}