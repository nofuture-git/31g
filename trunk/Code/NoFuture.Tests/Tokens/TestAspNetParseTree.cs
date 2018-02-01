﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Antlr.CSharp4;
using NoFuture.Antlr.HTML;

namespace NoFuture.Tests.Tokens
{
    [TestClass]
    public class TestAspNetParseTree
    {
        [TestMethod]
        public void TestPoc()
        {
            var testFile = TestAssembly.UnitTestsRoot + @"\ExampleDlls\AccountEdit.aspx";

            Assert.IsTrue(System.IO.File.Exists(testFile));

            var testResult = AspNetParseTree.InvokeParse(testFile);

            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Tags2Attrs);
            Assert.AreNotEqual(0,testResult.Tags2Attrs.Keys.Count);
            foreach (var key in testResult.Tags2Attrs.Keys)
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
            var testFile = TestAssembly.UnitTestsRoot + @"\ExampleDlls\SimpleExample.cs";

            Assert.IsTrue(System.IO.File.Exists(testFile));

            var testResult = CsharpParseTree.InvokeParse(testFile);

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
