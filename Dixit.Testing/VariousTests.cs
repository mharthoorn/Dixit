using Harthoorn.Dixit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dixit.Testing;

[TestClass]
public class VariousTests 
{
    [TestMethod]
    public void ErrorLine()
    {
        var file = new MemoryFile("abc def ghi");

        Token t = new Token(file, 4, 7, valid: false);
        var line = Nodes.GetTokenErrorLine(t);
        Assert.AreEqual("abc |def ghi", line);


        t = new Token(file, 11, 11, valid: false);
        line = Nodes.GetTokenErrorLine(t);
        Assert.AreEqual("abc def ghi|", line);

    }

    [TestMethod]
    public void Interlacing()
    {
        bool ok = Language.Compile("first, second, third", out _);
        Assert.IsTrue(ok);

        // unexpected glue at end:
        ok = Language.Compile("first, second, third, ", out _);
        Assert.IsFalse(ok);

        // no glue:
        ok = Language.Compile("first second", out _);
        Assert.IsFalse(ok);

        // count = 1, mincount = 2 
        ok = Language.Compile("first", out _);
        Assert.IsFalse(ok);

    }
}
