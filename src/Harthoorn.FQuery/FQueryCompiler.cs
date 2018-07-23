using System;
using System.Linq;
using Harthoorn.Dixit;
 
namespace Harthoorn.FQuery
{

    public class FQueryCompiler
    {

        public bool Compile(string text, out Node node)
        {
            var file = new MemoryFile(text);
            var compiler = new Compiler(FQL.Language);
            var success = compiler.Parse(file, out node);
            return success;
        }

        public (bool success, Node ast) Compile(string text)
        {
            var success = Compile(text, out Node ast);
            return (success, ast);
        }

        public Query GetQuery(Node node)
        {
            var nodes = node.DeepSelect(FQL.WhereClause, FQL.BooleanExpression, FQL.EqualityExpression).ToList();
            var dict = nodes.ToFilters();

            return new Query
            {
                Fields = node.DeepSelect(FQL.Statement, FQL.FieldList, FQL.Field, FQL.FieldName).Values().ToList(),
                Resource = node.DeepSelect(FQL.FromClause, FQL.FieldName).Values().FirstOrDefault(),
                Where = node.DeepSelect(FQL.WhereClause, FQL.BooleanExpression, FQL.EqualityExpression).ToFilters().ToList()
            };
        }

    }

}