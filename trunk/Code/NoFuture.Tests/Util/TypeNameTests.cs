using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Util
{
    [TestClass]
    public class TypeNameTests
    {
        [TestMethod]
        public void TestCtor()
        {
            var asmFullNameWithProcArch =
                "log4net, Version=1.2.13.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a, processorArchitecture=MSIL";

            var asmFullName =
                "log4net, Version=1.2.13.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a";

            var classAsmQualifiedName =
                "NoFuture.MyDatabase.Dbo.AccountExecutives, NoFuture.MyDatabase, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

            var nsClassName = "NoFuture.Util.TypeName";

            var testOne = new NoFuture.Util.NfTypeName(asmFullNameWithProcArch);
            var testTwo = new NoFuture.Util.NfTypeName(asmFullName);
            var testThree = new NoFuture.Util.NfTypeName(classAsmQualifiedName);
            var testFour = new NoFuture.Util.NfTypeName(nsClassName);

            Assert.IsTrue(string.IsNullOrWhiteSpace(testOne.AssemblyQualifiedName));
            Assert.IsTrue(string.IsNullOrWhiteSpace(testTwo.AssemblyQualifiedName));
            Assert.AreEqual(classAsmQualifiedName, testThree.AssemblyQualifiedName);
            Assert.IsTrue(string.IsNullOrWhiteSpace(testFour.AssemblyQualifiedName));

            Assert.AreEqual("log4net",testOne.AssemblyName);
            Assert.AreEqual("log4net",testTwo.AssemblyName);
            Assert.AreEqual("NoFuture.MyDatabase",testThree.AssemblyName);
            Assert.AreEqual(nsClassName,testFour.AssemblyName);

            Assert.AreEqual(asmFullNameWithProcArch, testOne.AssemblyFullName);
            Assert.AreEqual(asmFullName, testTwo.AssemblyFullName);
            Assert.AreEqual("NoFuture.MyDatabase, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",testThree.AssemblyFullName);
            Assert.AreEqual(nsClassName, testFour.AssemblyFullName);

            var testFive =
                new NoFuture.Util.NfTypeName(
                    "NoFuture.CRM.UI.Controller, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            Assert.AreEqual("NoFuture.CRM.UI.Controller.dll", testFive.AssemblyFileName);

            testThree =
                new NoFuture.Util.NfTypeName(
                    "NoFuture.MyDatabase.Dbo.AccountExecutives, NoFuture.MyDatabase, Version=0.0.0.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a");
            System.Diagnostics.Debug.WriteLine(string.Format("Original String: '{0}'", testThree.RawString));
            System.Diagnostics.Debug.WriteLine(string.Format("AssemblyFileName: '{0}'", testThree.AssemblyFileName));
            System.Diagnostics.Debug.WriteLine(string.Format("AssemblyFullName: '{0}'", testThree.AssemblyFullName));
            System.Diagnostics.Debug.WriteLine(string.Format("AssemblyName: '{0}'", testThree.AssemblyName));
            System.Diagnostics.Debug.WriteLine(string.Format("AssemblyQualifiedName: '{0}'", testThree.AssemblyQualifiedName));
            System.Diagnostics.Debug.WriteLine(string.Format("ClassName: '{0}'", testThree.ClassName));
            System.Diagnostics.Debug.WriteLine(string.Format("FullName: '{0}'", testThree.FullName));
            System.Diagnostics.Debug.WriteLine(string.Format("Namespace: '{0}'", testThree.Namespace));
            System.Diagnostics.Debug.WriteLine(string.Format("Version: '{0}'", testThree.Version));
            System.Diagnostics.Debug.WriteLine(string.Format("PublicKeyToken: '{0}'", testThree.PublicKeyToken));
            System.Diagnostics.Debug.WriteLine(string.Format("Culture: '{0}'", testThree.Culture));

            var testSix = new NoFuture.Util.NfTypeName("My_SillyName_NoNamespace_Pos_Class");
            System.Diagnostics.Debug.WriteLine(string.Format("Shitty Name Result :{0}", testSix.ClassName));
        }

        [TestMethod]
        public void TestSafeDotNetTypeName()
        {
            var testREsult = NoFuture.Util.NfTypeName.SafeDotNetTypeName("dbo.123ProcName");
            Assert.IsNotNull(testREsult);
            Assert.AreEqual("dbo.123ProcName",testREsult);
            System.Diagnostics.Debug.WriteLine(testREsult);

            testREsult = NoFuture.Util.NfTypeName.SafeDotNetTypeName(null);
            Assert.IsNotNull(testREsult);

            var testInput = string.Empty;
            testREsult = NoFuture.Util.NfTypeName.SafeDotNetTypeName(testInput);
            Assert.IsNotNull(testREsult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testREsult));
            System.Diagnostics.Debug.WriteLine(testREsult);

            testInput = "     ";
            testREsult = NoFuture.Util.NfTypeName.SafeDotNetTypeName(testInput);
            Assert.IsNotNull(testREsult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testREsult));
            System.Diagnostics.Debug.WriteLine(testREsult);

            testREsult = NoFuture.Util.NfTypeName.SafeDotNetTypeName("dbo.DELETED_LookupDetails");
        }

        [TestMethod]
        public void TestSafeDotNetIdentifier()
        {
            var testResult = NoFuture.Util.NfTypeName.SafeDotNetIdentifier("Personal Ph #",true);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("Personalx20Phx20x23",testResult);

            testResult = NoFuture.Util.NfTypeName.SafeDotNetIdentifier("Personal Ph #");
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("PersonalPh",testResult);

            testResult = NoFuture.Util.NfTypeName.SafeDotNetIdentifier("global::Some_Aspx_Page_With_No_Namespace");
            System.Diagnostics.Debug.WriteLine(testResult);

        }

        [TestMethod]
        public void TestGetTypeNameFromArrayAndGeneric()
        {
            var testResult = NoFuture.Util.NfTypeName.GetLastTypeNameFromArrayAndGeneric("int[]");
            Assert.AreEqual("int",testResult);
            testResult = NoFuture.Util.NfTypeName.GetLastTypeNameFromArrayAndGeneric("System.Collections.Generic.List`1[System.String]");
            Assert.AreEqual("System.String", testResult);
            testResult =
                NoFuture.Util.NfTypeName.GetLastTypeNameFromArrayAndGeneric("System.Tuple`2[System.Int32,System.String]");
            Assert.AreEqual("System.Int32",testResult);

            testResult = NoFuture.Util.NfTypeName.GetLastTypeNameFromArrayAndGeneric("System.Tuple`2[System.Collections.Generic.List`1[SomeNamespace.AType],System.String]");
            Assert.AreEqual("SomeNamespace.AType",testResult);

            //test C# style
            testResult =
                NoFuture.Util.NfTypeName.GetLastTypeNameFromArrayAndGeneric(
                    "System.Collections.Generic.List<System.String>", "<");
            Assert.AreEqual("System.String",testResult);

            testResult =
                NoFuture.Util.NfTypeName.GetLastTypeNameFromArrayAndGeneric("System.Tuple<System.Int32, System.String>", "<");
            Assert.AreEqual("System.Int32", testResult);

            testResult = NoFuture.Util.NfTypeName.GetLastTypeNameFromArrayAndGeneric("System.Tuple<System.Collections.Generic.List<SomeNamespace.AType>,System.String>", "<");
            Assert.AreEqual("SomeNamespace.AType", testResult);

            var testResults =
                NoFuture.Util.NfTypeName.GetTypeNamesFromGeneric(
                    "System.Tuple`3[System.Collections.Generic.List`1[System.String], System.String, System.Tuple`2[System.Int32, System.String]]");
            System.Diagnostics.Debug.WriteLine(string.Join(" | ", testResults));
        }

        [TestMethod]
        public void TestGetTypeNameWithoutNamespace()
        {
            var testResult = NoFuture.Util.NfTypeName.GetTypeNameWithoutNamespace("NoFuture.Asm.Fii.MyType");
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("MyType",testResult);

            testResult = NoFuture.Util.NfTypeName.GetTypeNameWithoutNamespace("My_SillyName_NoNamespace_Pos_Class");
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("My_SillyName_NoNamespace_Pos_Class", testResult);

            testResult =
                NoFuture.Util.NfTypeName.GetNamespaceWithoutTypeName(
                    "Wanker.DCF.UI.Controller.Operator.OperatorUIController::set_OperatorContacts(Wanker.DCF.DTO.WankerContact)");

            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual(testResult, "OperatorUIController");
        }

        [TestMethod]
        public void TestGetNamespaceWithoutTypeName()
        {
            var testResult = NoFuture.Util.NfTypeName.GetNamespaceWithoutTypeName("NoFuture.Asm.Fii.MyType");
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("NoFuture.Asm.Fii", testResult);

            testResult = NoFuture.Util.NfTypeName.GetNamespaceWithoutTypeName("My_SillyName_NoNamespace_Pos_Class");
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNull(testResult);

            testResult =
                NoFuture.Util.NfTypeName.GetNamespaceWithoutTypeName(
                    "Wanker.DCF.UI.Controller.Operator.OperatorUIController::set_OperatorContacts(Wanker.DCF.DTO.WankerContact)");

            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual(testResult, "Wanker.DCF.UI.Controller.Operator");

            testResult =
                NoFuture.Util.NfTypeName.GetNamespaceWithoutTypeName(
                    ".Controller.Operator.OperatorUIController::set_OperatorContacts(Wanker.DCF.DTO.WankerContact)");

            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual(testResult, "Controller.Operator");
        }

        [TestMethod]
        public void TestIsClrMethodForProperty()
        {
            var testInput = "get_MyProperty";
            string testOutput;
            var testResult = NoFuture.Util.NfTypeName.IsClrMethodForProperty(testInput, out testOutput);

            Assert.IsTrue(testResult);
            Assert.AreEqual("MyProperty",testOutput);

            testInput = "set_MyProperty";
            testResult = NoFuture.Util.NfTypeName.IsClrMethodForProperty(testInput, out testOutput);

            Assert.IsTrue(testResult);
            Assert.AreEqual("MyProperty", testOutput);

            testInput = "add_MyProperty";
            testResult = NoFuture.Util.NfTypeName.IsClrMethodForProperty(testInput, out testOutput);

            Assert.IsTrue(testResult);
            Assert.AreEqual("MyProperty", testOutput);

            testInput = "remove_MyProperty";
            testResult = NoFuture.Util.NfTypeName.IsClrMethodForProperty(testInput, out testOutput);

            Assert.IsTrue(testResult);
            Assert.AreEqual("MyProperty", testOutput);

            testInput = "GetMyValues";
            testResult = NoFuture.Util.NfTypeName.IsClrMethodForProperty(testInput, out testOutput);

            Assert.IsFalse(testResult);
            Assert.IsNull(testOutput);
        }
    }

}
