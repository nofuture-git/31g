using System;
using NUnit.Framework;

namespace NoFuture.Tests.Antlr
{
    [TestFixture]
    public class TestCsharpParseTree
    {
        [Test]
        public void TestInvokeParse()
        {
            var testFile = System.IO.Path.Combine(TestAssembly.UnitTestsRoot,
                @"ExampleDlls\AdventureWorks2012\IndependentDll\Class00.cs");
            if(!System.IO.File.Exists(testFile))
                Assert.Fail($"missing test file at {testFile}");

            var testResult =
                NoFuture.Antlr.CSharp4.CsharpParseTree.InvokeParse(testFile);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.ClassNames.Count);
            Assert.AreNotEqual(0, testResult.MethodNames);

            foreach(var n in testResult.ClassNames)
                Console.WriteLine(n);
        }
    }
}
