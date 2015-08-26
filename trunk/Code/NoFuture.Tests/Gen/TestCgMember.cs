using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Gen;

namespace NoFuture.Tests.Gen
{
    [TestClass]
    public class TestCgMember
    {
        [TestMethod]
        public void TestToCsDecl()
        {
            var testSubject = new CgMember()
            {
                Args =
                    new List<CgArg>
                    {
                        new CgArg {ArgName = "param1", ArgType = "int"},
                        new CgArg {ArgName = "param2", ArgType = "string"},
                        new CgArg {ArgName = "param3", ArgType = "bool"}
                    },
                Name = "MyMethodName",
                TypeName =  "System.String"
            };

            var testResult = NoFuture.Gen.Settings.LangStyle.ToDecl(testSubject);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult);
            Assert.IsTrue(testResult.StartsWith(testSubject.TypeName));
            System.Diagnostics.Debug.WriteLine(testResult);

            testSubject.TypeName = "MyNamespace.Here.MyTypeName";
            testSubject.Name = ".ctor";
            testSubject.IsCtor = true;
            testResult = NoFuture.Gen.Settings.LangStyle.ToDecl(testSubject);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        
        }

        [TestMethod]
        public void TestToCsStmt()
        {
            var testSubject = new CgMember()
            {
                Args =
                    new List<CgArg>
                    {
                        new CgArg {ArgName = "param1", ArgType = "int"},
                        new CgArg {ArgName = "param2", ArgType = "string"},
                        new CgArg {ArgName = "param3", ArgType = "bool"}
                    },
                Name = "MyMethodName",
                TypeName = "System.String"
            };
            var testResult = NoFuture.Gen.Settings.LangStyle.ToStmt(testSubject, "MyNamespace.Here", "MyTypeName");
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult);
            Assert.IsTrue(testResult.StartsWith("return"));

            testSubject.TypeName = "void";
            testResult = NoFuture.Gen.Settings.LangStyle.ToStmt(testSubject, "MyNamespace.Here", "MyTypeName");
            System.Diagnostics.Debug.WriteLine(testResult);

            Assert.IsTrue(testResult.StartsWith("MyNamespace"));

            testSubject.TypeName = "MyNamespace.Here.MyTypeName";
            testSubject.Name = ".ctor";
            testSubject.IsCtor = true;
            testResult = NoFuture.Gen.Settings.LangStyle.ToStmt(testSubject, null, null);
            Assert.IsNotNull(testResult);
            Assert.AreEqual("new MyNamespace.Here.MyTypeName(param1,param2,param3);", testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestAsInvokeRegexPattern()
        {
            var testSubject = new CgMember()
            {
                Args =
                    new List<CgArg>
                    {
                        new CgArg {ArgName = "param1", ArgType = "int"},
                        new CgArg {ArgName = "param2", ArgType = "string"},
                        new CgArg {ArgName = "param3", ArgType = "bool"}
                    },
                Name = "MyMethodName",
                TypeName = "System.String"
            };
            var testResult = testSubject.AsInvokeRegexPattern();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult);
            Assert.AreNotEqual(".",testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch("MyMethodName(11,\"a string literal\",true)",testResult));

            testSubject.IsStatic = true;
            testSubject.MyCgType = new CgType {Namespace = "NoFuture.TestNs", Name = "Something"};
            testSubject.IsGeneric = true;
            testResult = testSubject.AsInvokeRegexPattern();
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);

            testSubject.IsStatic = false;
            testResult = testSubject.AsInvokeRegexPattern("myVar", "myVar.ItsTypes.Types.Type");
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);

            testSubject.MyCgType.IsEnum = true;
            testSubject.IsGeneric = false;
            testSubject.Name = "MyEnum";
            testSubject.MyCgType.EnumValueDictionary.Add("MyType", new[] { "Instance", "NonPublic", "Public", "DeclaredOnly" });
            testResult = testSubject.AsInvokeRegexPattern();
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);

            testSubject = new CgMember()
            {
                Name = "MyPropertyName",
                TypeName = "System.String"
            };

            testResult = testSubject.AsInvokeRegexPattern();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult);
            Assert.AreNotEqual(".", testResult);
            System.Diagnostics.Debug.WriteLine(testResult);

        }
    }
}
