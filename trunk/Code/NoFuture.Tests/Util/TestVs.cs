using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Util;

namespace NoFuture.Tests.Util
{
    [TestClass]
    public class TestVs
    {
        [TestMethod]
        public void TestToProjRelPath()
        {
            var testInput = @"admin\SomeFile.fs";
            var testResult = Etc.ToRelPath(@"C:\Projects\MyProject", ref testInput);
            Assert.IsFalse(testResult);
            Assert.AreEqual(@"admin\SomeFile.fs", testInput);

            testInput = @"C:\Projects\MyProject\admin\SomeFile.fs";
            testResult = Etc.ToRelPath(@"C:\Projects\MyProject", ref testInput);
            Assert.IsTrue(testResult);
            Assert.AreEqual(@"admin\SomeFile.fs", testInput);

            testInput = @"C:\Projects\MyProject\AnotherFolder\admin\SomeFile.fs";
            testResult = Etc.ToRelPath(@"C:\Projects\MyProject", ref testInput );
            Assert.IsTrue(testResult);
            Assert.AreEqual(@"AnotherFolder\admin\SomeFile.fs", testInput);

            testInput = @"C:\Projects\DiffProj\admin\SomeFile.fs";
            testResult = Etc.ToRelPath(@"C:\Projects\MyProject", ref testInput);
            Assert.IsFalse(testResult);
            Assert.AreEqual(@"C:\Projects\DiffProj\admin\SomeFile.fs", testInput);
        }
    }
}
