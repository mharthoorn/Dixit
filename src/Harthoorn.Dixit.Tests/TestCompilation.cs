using System;
using Xunit;
using Harthoorn.FQuery;
using System.Linq;

namespace Harthoorn.Dixit.Tests
{
    public class TestCompilation
    {
        FQueryCompiler compiler = new FQueryCompiler();


        [Fact]
        public void BasicSelect()
        {
            string s = "select * from users";
            (Node node, bool success) = compiler.Compile(s);
            Assert.True(success);
            var fields = node.Descend(FQL.Statement, FQL.FieldList, FQL.Wildcard).ToList();
            Assert.True(fields[0].Text == "*");
        }

        [Fact]
        public void FieldList()
        {
            string s = "select aap, noot, mies from users ";
            (Node node, bool success) = compiler.Compile(s);
            Assert.True(success);
            var fields = node.Descend(FQL.Statement, FQL.FieldList, FQL.FieldName).ToList();
            Assert.True(fields[0].Text == "aap");
            Assert.True(fields[1].Text == "noot");
            Assert.True(fields[2].Text == "mies");
        }

        [Fact]
        public void WhereClause()
        {
            string s = "select aap, noot, mies from users where id = '4' and name = 'John' ";
            (Node node, bool success) = compiler.Compile(s);
            Assert.True(success);

            var fields = node.Descend(FQL.Statement, FQL.WhereClause, FQL.FieldName).ToList();
            Assert.True(fields[0].Text == "id");
            Assert.True(fields[1].Text == "name");

            var values = node.Descend(FQL.Statement, FQL.WhereClause, FQL.StringValue).ToList();
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
            (Node node, bool success) = compiler.Compile(query);
            Assert.True(success);
        }

        [Fact]
        public void ErrorInOptional()
        {
            string s = "select aap, noot, mies from users where @#$ ";
            (Node node, bool success) = compiler.Compile(s);
            Assert.Equal(State.Invalid, node.State);
            Assert.False(success);
            // the optional where clause should still fail the whole.
            // it should not end with "unexpected characters at end of statement "where id =..."
        }
    }
}
