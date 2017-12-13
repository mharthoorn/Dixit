using System;
using Xunit;
using Harthoorn.Dixit.Sql;
using System.Linq;

namespace Harthoorn.Dixit.Tests
{
    public class TestCompilation
    {
        SqlCompiler compiler = new SqlCompiler();

        
        [Fact]
        public void BasicSelect()
        {
            string s = "select * from users";
            (Node node, bool success) = compiler.Compile(s);
            Assert.True(success);
            var fields = node.Descend(SQL.Statement, SQL.FieldList, SQL.Wildcard).ToList();
            Assert.True(fields[0].Text == "*");
        }

        [Fact]
        public void FieldList()
        {
            string s = "select aap, noot, mies from users ";
            (Node node, bool success) = compiler.Compile(s);
            Assert.True(success);
            var fields = node.Descend(SQL.Statement, SQL.FieldList, SQL.FieldName).ToList();
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
            var fields = node.Descend(SQL.Statement, SQL.WhereClause, SQL.FieldName).ToList();
            Assert.True(fields[0].Text == "id");
            Assert.True(fields[1].Text == "name");

            var values = node.Descend(SQL.Statement, SQL.WhereClause, SQL.StringValue).ToList();
            Assert.True(values[0].Text == "4");
            Assert.True(values[1].Text == "John");
        }

        [Fact]
        public void BrokenWhereClause()
        {
            string s = "select aap, noot, mies from users where id = '4', name = 'John' ";
            (Node node, bool success) = compiler.Compile(s);
            Assert.False(success);
            // the optional where clause should still fail the whole.
            // it should not end with "unexpected characters at end of statement "where id =..."


            //var fields = node.Descend(SQL.Statement, SQL.WhereClause, SQL.FieldName).ToList();
            //Assert.True(fields[0].Text == "id");
            //Assert.True(fields[1].Text == "name");

            //var values = node.Descend(SQL.Statement, SQL.WhereClause, SQL.StringValue).ToList();
            //Assert.True(fields[0].Text == "4");
            //Assert.False(fields[1].Text == "John");

        }
    }
}
