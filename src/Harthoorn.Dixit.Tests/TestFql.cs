using System;
using Xunit;
using Harthoorn.FQuery;
using System.Linq;

namespace Harthoorn.Dixit.Tests
{
    public class TestFql
    {
        FQLCompiler compiler = new FQLCompiler();


        [Fact]
        public void BasicSelect()
        {
            string s = "select * from users";
            bool success = compiler.Compile(s, out Node node);
            Assert.True(success);
            var fields = node.DeepSelect(FQL.Statement, FQL.FieldList, FQL.Wildcard).ToList();
            Assert.True(fields[0].Text == "*");
        }

        [Fact]
        public void FieldList()
        {
            string s = "select aap, noot, mies from users ";
            bool success = compiler.Compile(s, out Node node);
            Assert.True(success);
            var fields = node.DeepSelect(FQL.Statement, FQL.FieldList, FQL.FieldName).ToList();
            Assert.True(fields[0].Text == "aap");
            Assert.True(fields[1].Text == "noot");
            Assert.True(fields[2].Text == "mies");
        }

        [Fact]
        public void WhereClause()
        {
            string s = "select aap, noot, mies from users where id = '4' and name = 'John' ";
            bool success = compiler.Compile(s, out Node node);
            Assert.True(success);

            var fields = node.DeepSelect(FQL.Statement, FQL.WhereClause, FQL.FieldName).ToList();
            Assert.True(fields[0].Text == "id");
            Assert.True(fields[1].Text == "name");

            var values = node.DeepSelect(FQL.Statement, FQL.WhereClause, FQL.StringValue).ToList();
            Assert.True(values[0].Text == "4");
            Assert.True(values[1].Text == "John");
        }

        [Fact]
        public void WhiteSpace()
        {
            string query = @"select
                    id,
                    gender,
                    name
                from 
                    Patient";
            bool success = compiler.Compile(query, out Node node);
            Assert.True(success);
        }

        [Fact]
        public void ErrorInOptional()
        {
            string s = "select aap, noot, mies from users where @#$ ";
            bool success = compiler.Compile(s, out Node node);
            Assert.Equal(State.Invalid, node.State);
            Assert.False(success);
            // the optional where clause should still fail the whole.
            // it should not end with "unexpected characters at end of statement "where id =..."
        }

        [Fact]
        public void JsonFields()
        {
            string query = @"
                select
                    field1 : 'value1',
                    field2 : statement1,
                    field3,
                    field4 : statement2
                from
                    Table
            ";
            (bool success, Node node) = compiler.Compile(query);
            Assert.True(success);

            var fields = node.DeepSelect(FQL.Statement, FQL.FieldList, FQL.FieldName).ToList();
            Assert.True(fields[0].Text == "field1");
            Assert.True(fields[1].Text == "field2");
            Assert.True(fields[2].Text == "field3");
            Assert.True(fields[3].Text == "field4");


            var expressions = node.DeepSelect(FQL.Statement, FQL.FieldList, FQL.Projection, FQL.FieldExpression).ToList();
            Assert.True(expressions[0].Text == "'value1'");
            Assert.True(expressions[1].Text == "statement1");
            Assert.True(expressions[2].Text == "statement2");

        }

        [Fact]
        public void CombinedFqlFhirPath()
        {
            string query = @"
                select
                    Patient: {
                        name : 'John',
                        family: 'Williams',
                        given: Patient.given
                    }
                from
                    Patient
            ";
            (bool success, Node node) = compiler.Compile(query);
            Assert.True(success);

            var fields = node.DeepSelect(FQL.Statement, FQL.FieldList, FQL.Field).ToList();

            Assert.True(fields.Count() == 1);
            var name = fields[0].PathSelect(FQL.Projection, FQL.FieldName).ToList();
            Assert.True(name[0].Text == "Patient");

            var expressions = fields.PathSelect(FQL.FieldList, FQL.Projection, FQL.FieldExpression).ToList();
            Assert.True(expressions[0].Text == "'value1'");
            Assert.True(expressions[1].Text == "statement1");
            Assert.True(expressions[2].Text == "statement2");

        }


    }
}
