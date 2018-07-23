using Xunit;
using Harthoorn.FQuery;
using System.Linq;

namespace Harthoorn.Dixit.Tests
{
    public class TestNodeLinq
    {
        FQueryCompiler compiler = new FQueryCompiler();

        [Fact]
        public void PathSelecting()
        {
            string query = @"
                select
                    field1 : { 
                        subfield1 : 'value1',
                        subfield2 : statement1,
                        subfield3 : 'value2'
                    }, 
                    field2 : statement2
                from
                    Table
            ";
            (bool success, Node node) = compiler.Compile(query);
            Assert.True(success);

            var fields = node.DeepSelect(FQL.Statement, FQL.FieldList, FQL.Field).ToList();
            var fieldnames = fields.PathSelect(FQL.Projection, FQL.FieldName).ToList();
            Assert.True(fieldnames[0].Text == "field1");
            Assert.True(fieldnames[1].Text == "field2");

            var subfields = fields.DeepSelect(FQL.FieldList, FQL.FieldName).ToList();
            Assert.True(subfields[0].Text == "subfield1");
            Assert.True(subfields[1].Text == "subfield2");
            Assert.True(subfields[2].Text == "subfield3");



        }

        [Fact]
        public void DeepSelecting()
        {
            string query = @"
                select
                    field1 : { 
                        subfield1 : 'value1',
                        subfield2 : statement1,
                        subfield3 : 'value2'
                    }, 
                    field2 : statement2
                from
                    Table
            ";
            (bool success, Node node) = compiler.Compile(query);
            Assert.True(success);

            var expressions = node.DeepSelect(FQL.FieldExpression).ToList();
            Assert.True(expressions[0].Text == "'value1'");
            Assert.True(expressions[1].Text == "statement1");
            Assert.True(expressions[2].Text == "'value2'");
            Assert.True(expressions[3].Text == "statement2");
        }

    }
}
