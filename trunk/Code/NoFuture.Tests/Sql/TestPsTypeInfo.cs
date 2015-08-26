using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace NoFuture.Tests.Sql
{
    [TestClass]
    public class TestPsTypeInfo
    {
        [TestMethod]
        public void TestCtor()
        {
            var testInputAllBoolStrings = new List<string>
            {
                bool.TrueString,
                bool.TrueString,
                bool.FalseString,
                bool.TrueString,
                bool.FalseString,
                bool.FalseString
            };

            var testInputAllBoolMssqlBit = new List<string> {"1", "0", "0", "1", "0", "1"};
            var testInputAllDecimal = new List<string> {"0.0", "1.0", "3.4", "6.90", "8.098"};
            var testInputAllInt = new List<string> {"2", "138", "56", "85", "-5"};
            var testInputAllDate = new List<string>
            {
                DateTime.Today.ToLongDateString(),
                DateTime.Today.ToShortDateString(),
                "2015-03-04 09:23:55.554121"
            };

            var testResult = new NoFuture.Sql.Mssql.Md.PsTypeInfo("test", testInputAllBoolStrings.ToArray());
            Assert.AreEqual(NoFuture.Sql.Mssql.Md.PsTypeInfo.BOOL, testResult.PsDbType);

            testResult = new NoFuture.Sql.Mssql.Md.PsTypeInfo("test", testInputAllBoolMssqlBit.ToArray());
            Assert.AreEqual(NoFuture.Sql.Mssql.Md.PsTypeInfo.BOOL, testResult.PsDbType);

            testResult = new NoFuture.Sql.Mssql.Md.PsTypeInfo("test", testInputAllDecimal.ToArray());
            Assert.AreEqual(NoFuture.Sql.Mssql.Md.PsTypeInfo.DEC, testResult.PsDbType);

            testResult = new NoFuture.Sql.Mssql.Md.PsTypeInfo("test", testInputAllInt.ToArray());
            Assert.AreEqual(NoFuture.Sql.Mssql.Md.PsTypeInfo.INT, testResult.PsDbType);

            testResult = new NoFuture.Sql.Mssql.Md.PsTypeInfo("test", testInputAllDate.ToArray());
            Assert.AreEqual(NoFuture.Sql.Mssql.Md.PsTypeInfo.DATE, testResult.PsDbType);

            var testInputMixed = new List<string>();
            testInputMixed.AddRange(testInputAllBoolMssqlBit);
            testInputMixed.Add("do you wanna earn more money");
            testInputMixed.Add("sure we all do.");

            testResult = new NoFuture.Sql.Mssql.Md.PsTypeInfo("test", testInputMixed.ToArray());
            Assert.IsTrue(testResult.PsDbType.StartsWith(NoFuture.Sql.Mssql.Md.PsTypeInfo.DEFAULT_PS_DB_TYPE));

            var testInputAllBoolMixed = new List<string>();
            testInputAllBoolMixed.AddRange(testInputAllBoolStrings);
            testInputAllBoolMixed.AddRange(testInputAllBoolMssqlBit);

            testResult = new NoFuture.Sql.Mssql.Md.PsTypeInfo("test", testInputAllBoolMixed.ToArray());
            Assert.AreEqual(NoFuture.Sql.Mssql.Md.PsTypeInfo.BOOL, testResult.PsDbType);

            var testInputAllIntWithDecPt = new List<string> {"2.0", "138.0", "56.0", "85.0", "-5.0"};
            testResult = new NoFuture.Sql.Mssql.Md.PsTypeInfo("test", testInputAllIntWithDecPt.ToArray());
            Assert.AreEqual(NoFuture.Sql.Mssql.Md.PsTypeInfo.DEC, testResult.PsDbType);

        }
    }
}
