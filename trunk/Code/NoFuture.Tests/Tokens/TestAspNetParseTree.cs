using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Tokens
{
    [TestClass]
    public class TestAspNetParseTree
    {
        [TestMethod]
        public void TestPoc()
        {
            var testFile = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\AccountEdit.aspx";

            Assert.IsTrue(System.IO.File.Exists(testFile));

            var testResult = NoFuture.Tokens.AspNetParseTree.InvokeParse(testFile);

            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.DistinctTags);
            Assert.AreNotEqual(0,testResult.DistinctTags.Keys.Count);
            foreach (var key in testResult.DistinctTags.Keys)
            {
                //var attrs = string.Join("|", testResult.DistinctTags[key]);
                System.Diagnostics.Debug.WriteLine(key);
            }

            Assert.IsNotNull(testResult.ScriptBodies);
            Assert.AreNotEqual(0,testResult.ScriptBodies.Count);
            foreach (var script in testResult.ScriptBodies)
            {
                System.Diagnostics.Debug.WriteLine(script);
            }
        }

        [TestMethod]
        public void TestCsharpParse()
        {
            var testFile = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\SimpleExample.cs";

            Assert.IsTrue(System.IO.File.Exists(testFile));

            var testResult = NoFuture.Tokens.CsharpParseTree.InvokeParse(testFile);

            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.CatchBlocks);
            Assert.AreNotEqual(0, testResult.CatchBlocks.Count);

            foreach (var c in testResult.CatchBlocks)
            {
                System.Diagnostics.Debug.WriteLine(c);
            }
        }
    }
}
