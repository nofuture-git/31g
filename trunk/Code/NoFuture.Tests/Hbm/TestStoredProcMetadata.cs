using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Hbm.SortingContainers;

namespace NoFuture.Tests.Hbm
{
    [TestClass]
    public class TestStoredProcMetadata
    {
        [TestMethod]
        public void TestToHbmSql()
        {
            var testSubject = new NoFuture.Hbm.SortingContainers.StoredProcMetadata();
            testSubject.ProcName = "dbo.MyStoredProc";
            var testResult = testSubject.ToHbmSql();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsTrue(testResult.Contains("EXEC [dbo].[MyStoredProc]"));

            testSubject.ProcName = "MyStoredProc";
            testResult = testSubject.ToHbmSql();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsTrue(testResult.Contains("EXEC [MyStoredProc]"));

            testSubject.ProcName = "my.dot.schema.MyStoredProc";
            testResult = testSubject.ToHbmSql();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsTrue(testResult.Contains("EXEC [my.dot.schema].[MyStoredProc]"));

            testSubject.ProcName = "MyStoredProc.";
            testResult = testSubject.ToHbmSql();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsTrue(testResult.Contains("EXEC [MyStoredProc]"));

            testSubject.ProcName = ".MyStoredProc";
            testResult = testSubject.ToHbmSql();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsTrue(testResult.Contains("EXEC [MyStoredProc]"));

            testSubject.ProcName = ".MyStoredProc.";
            testResult = testSubject.ToHbmSql();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsTrue(testResult.Contains("EXEC [MyStoredProc]"));

            testSubject.Parameters = new List<StoredProcParamItem>
            {
                new StoredProcParamItem {ParamName = "@parameter1"},
                new StoredProcParamItem {ParamName = "@parameter2"},
                new StoredProcParamItem {ParamName = "@parameter3"}
            };

            testResult = testSubject.ToHbmSql();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsTrue(testResult.Contains("EXEC [MyStoredProc]"));
            Assert.IsTrue(testResult.Contains(":parameter1,"));
            Assert.IsTrue(testResult.Contains(":parameter2,"));
            Assert.IsTrue(testResult.Contains(":parameter3"));

        }

        [TestMethod]
        public void TestTryParseToHbmSql()
        {
            var tesSubject = @"

EXEC [MyStoredProc]
	:parameter1,
	:parameter2,
	:parameter3

";
            string testOutDbName;
            string[] testOutParamNames;
            var testResult = StoredProcMetadata.TryParseToHbmSql(tesSubject,
                out testOutDbName, out testOutParamNames);

            Assert.IsTrue(testResult);
            Assert.IsNotNull(testOutDbName);
            Assert.IsNotNull(testOutParamNames);
            Assert.AreNotEqual(0, testOutParamNames.Length);

            System.Diagnostics.Debug.WriteLine(string.Format("Db Name: '{0}'", testOutDbName));
            foreach (var testOutPn in testOutParamNames)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Param Name: '{0}'", testOutPn));
            }

        }

        [TestMethod]
        public void TestDotNetType()
        {
            var testSubject = new StoredProcParamItem {DataType = "datetime", IsNullable = true};

            var testResult = testSubject.DotNetType;
            Assert.IsNotNull(testResult);
            Assert.AreEqual("System.Nullable<System.DateTime>",testResult);

        }
    }
}
