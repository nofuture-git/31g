using System;
using NUnit.Framework;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util;
using NoFuture.Util.Core;
using NoFuture.Util.NfType;

namespace NoFuture.Tests.Util.NfTypeTests
{
    [TestFixture]
    public class TypeNameTests
    {
        [SetUp]
        public void Init()
        {
            NfConfig.CustomTools.InvokeNfTypeName =
                @"C:\Projects\31g\trunk\Code\NoFuture\bin\NoFuture.Tokens.InvokeNfTypeName.exe";
        }

        [Test]
        public void TestCtor()
        {
            var asmFullNameWithProcArch =
                "log4net, Version=1.2.13.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a, processorArchitecture=MSIL";

            var asmFullName =
                "log4net, Version=1.2.13.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a";

            var classAsmQualifiedName =
                "NoFuture.MyDatabase.Dbo.AccountExecutives, NoFuture.MyDatabase, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

            var nsClassName = "NoFuture.Util.TypeName";

            var testOne = new NfTypeName(asmFullNameWithProcArch);
            var testTwo = new NfTypeName(asmFullName);
            var testThree = new NfTypeName(classAsmQualifiedName);
            var testFour = new NfTypeName(nsClassName);

            Console.WriteLine(testOne.AssemblyQualifiedName);
            Assert.IsTrue(string.IsNullOrWhiteSpace(testOne.AssemblyQualifiedName));

            Assert.IsTrue(string.IsNullOrWhiteSpace(testTwo.AssemblyQualifiedName));
            Assert.AreEqual(classAsmQualifiedName, testThree.AssemblyQualifiedName);
            Assert.IsTrue(string.IsNullOrWhiteSpace(testFour.AssemblyQualifiedName));

            Assert.AreEqual("log4net",testOne.AssemblySimpleName);
            Assert.AreEqual("log4net",testTwo.AssemblySimpleName);
            Assert.AreEqual("NoFuture.MyDatabase",testThree.AssemblySimpleName);
            Assert.AreEqual(nsClassName,testFour.AssemblySimpleName);

            Assert.AreEqual(asmFullName, testTwo.AssemblyFullName);
            Assert.AreEqual("NoFuture.MyDatabase, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",testThree.AssemblyFullName);

            var testFive =
                new NfTypeName(
                    "NoFuture.CRM.UI.Controller, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            Assert.AreEqual("NoFuture.CRM.UI.Controller.dll", testFive.AssemblyFileName);

            testThree =
                new NfTypeName(
                    "NoFuture.MyDatabase.Dbo.AccountExecutives, NoFuture.MyDatabase, Version=0.0.0.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a");
            Console.WriteLine($"Original String: '{testThree.RawString}'");
            Assert.AreEqual("NoFuture.MyDatabase.dll", testThree.AssemblyFileName);
            Assert.AreEqual("NoFuture.MyDatabase, Version=0.0.0.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a", testThree.AssemblyFullName);
            Assert.AreEqual("NoFuture.MyDatabase",testThree.AssemblySimpleName);
            Assert.AreEqual(testThree.RawString, testThree.AssemblyQualifiedName);
            Assert.AreEqual("AccountExecutives", testThree.ClassName);
            Assert.AreEqual("NoFuture.MyDatabase.Dbo.AccountExecutives", testThree.FullName);
            Assert.AreEqual("NoFuture.MyDatabase.Dbo",testThree.Namespace);
            Assert.AreEqual("Version=0.0.0.0", testThree.Version);
            Assert.AreEqual("PublicKeyToken=669e0ddf0bb1aa2a", testThree.PublicKeyToken);
            Assert.AreEqual("Culture=neutral", testThree.Culture);

            var testSix = new NfTypeName("My_SillyName_NoNamespace_Pos_Class");
            Assert.AreEqual("My_SillyName_NoNamespace_Pos_Class", testSix.ClassName);
        }

        [Test]
        public void TestSafeDotNetTypeName()
        {
            var testResult = Etc.SafeDotNetTypeName("dbo.123ProcName");
            Assert.IsNotNull(testResult);
            Assert.AreEqual("dbo.123ProcName",testResult);
            Console.WriteLine(testResult);

            testResult = Etc.SafeDotNetTypeName(null);
            Assert.IsNotNull(testResult);

            var testInput = string.Empty;
            testResult = Etc.SafeDotNetTypeName(testInput);
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            Console.WriteLine(testResult);

            testInput = "     ";
            testResult = Etc.SafeDotNetTypeName(testInput);
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            Console.WriteLine(testResult);

            testResult = Etc.SafeDotNetTypeName("dbo.DELETED_LookupDetails");
            Console.WriteLine(testResult);
            Assert.AreEqual("dbo.DELETED_LookupDetails", testResult);

            testResult = Etc.SafeDotNetTypeName("Â© The End");

            Console.WriteLine(testResult);

        }

        [Test]
        public void TestSafeDotNetIdentifier()
        {
            var testResult = Etc.SafeDotNetIdentifier("Personal Ph #",true);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual("Personal_u0020Ph_u0020_u0023", testResult);

            testResult = Etc.SafeDotNetIdentifier("Personal Ph #");
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual("PersonalPh",testResult);

            testResult = Etc.SafeDotNetIdentifier("global::Some_Aspx_Page_With_No_Namespace");
            Console.WriteLine(testResult);

            Assert.AreEqual("globalSome_Aspx_Page_With_No_Namespace", testResult);

            testResult =
                Etc.SafeDotNetIdentifier(
                    "<p><font style='font-size:11px;font-family:calibri;text-align:left'>", true);
            Console.WriteLine(testResult);

            Assert.IsTrue(testResult.StartsWith(Etc.DefaultNamePrefix + "_u003cp_u003e_u003cfont_u0020style"));

            testResult =
                Etc.SafeDotNetIdentifier("Â© The End Â©", false);
            Console.WriteLine(testResult);

            Assert.AreEqual("TheEnd",testResult);

            testResult =
                Etc.SafeDotNetIdentifier("Â© The End Â©", true);
            Console.WriteLine(testResult);

            Assert.AreEqual("_u00c2_u00a9_u0020The_u0020End_u0020_u00c2_u00a9", testResult);
        }

        [Test]
        public void TestGetTypeNameFromArrayAndGeneric()
        {
            var testResult = NfReflect.GetLastTypeNameFromArrayAndGeneric("int[]");
            Assert.AreEqual("int",testResult);
            testResult = NfReflect.GetLastTypeNameFromArrayAndGeneric("System.Collections.Generic.List`1[System.String]");
            Assert.AreEqual("System.String", testResult);
            testResult =
                NfReflect.GetLastTypeNameFromArrayAndGeneric("System.Tuple`2[System.Int32,System.String]");
            Assert.AreEqual("System.Int32",testResult);

            testResult = NfReflect.GetLastTypeNameFromArrayAndGeneric("System.Tuple`2[System.Collections.Generic.List`1[SomeNamespace.AType],System.String]");
            Assert.AreEqual("SomeNamespace.AType",testResult);

            //test C# style
            testResult =
                NfReflect.GetLastTypeNameFromArrayAndGeneric(
                    "System.Collections.Generic.List<System.String>", "<");
            Assert.AreEqual("System.String",testResult);

            testResult =
                NfReflect.GetLastTypeNameFromArrayAndGeneric("System.Tuple<System.Int32, System.String>", "<");
            Assert.AreEqual("System.Int32", testResult);

            testResult = NfReflect.GetLastTypeNameFromArrayAndGeneric("System.Tuple<System.Collections.Generic.List<SomeNamespace.AType>,System.String>", "<");
            Assert.AreEqual("SomeNamespace.AType", testResult);

            var testResults =
                NfReflect.GetTypeNamesFromGeneric(
                    "System.Tuple`3[System.Collections.Generic.List`1[System.String], System.String, System.Tuple`2[System.Int32, System.String]]");
            Console.WriteLine(string.Join(" | ", testResults));
        }

        [Test]
        public void TestGetTypeNameWithoutNamespace()
        {
            var testResult = NfReflect.GetTypeNameWithoutNamespace("NoFuture.Asm.Fii.MyType");
            Console.WriteLine(testResult);
            Assert.AreEqual("MyType",testResult);

            testResult = NfReflect.GetTypeNameWithoutNamespace("My_SillyName_NoNamespace_Pos_Class");
            Console.WriteLine(testResult);
            Assert.AreEqual("My_SillyName_NoNamespace_Pos_Class", testResult);

            testResult =
                NfReflect.GetNamespaceWithoutTypeName(
                    "Wanker.DCF.UI.Controller.Operator.OperatorUIController::set_OperatorContacts(Wanker.DCF.DTO.WankerContact)");

            Console.WriteLine(testResult);
            Assert.AreEqual(testResult, "OperatorUIController");

        }

        [Test]
        public void TestGetNamespaceWithoutTypeName()
        {
            var testResult = NfReflect.GetNamespaceWithoutTypeName("NoFuture.Asm.Fii.MyType");
            Console.WriteLine(testResult);
            Assert.AreEqual("NoFuture.Asm.Fii", testResult);

            var testResult2 = NfReflect.GetNamespaceWithoutTypeName("My_SillyName_NoNamespace_Pos_Class");
            
            Assert.IsNull(testResult2);

            testResult =
                NfReflect.GetNamespaceWithoutTypeName(
                    "global::Wanker.DCF.UI.Controller.Operator.OperatorUIController::set_OperatorContacts(Wanker.DCF.DTO.WankerContact)");
            Console.WriteLine(testResult);
            Assert.AreEqual("Wanker.DCF.UI.Controller.Operator",testResult);

            testResult = NfReflect.GetNamespaceWithoutTypeName("NeedItInIl.DomainAdapterBase`2[[AThirdDll.Whatever, AThirdDll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]," +
                                                                "[System.Tuple`3[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]," +
                                                                "[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]," +
                                                                "[System.Collections.Generic.IEnumerable`1[[MoreBinaries.DomainObject+MyInnerType, MoreBinaries, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]," +
                                                                " mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], " +
                                                                "NeedItInIl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");


            Console.WriteLine(testResult);
            Assert.AreEqual("NeedItInIl", testResult);

            testResult =
                NfReflect.GetNamespaceWithoutTypeName(
                    "Wanker.DCF.UI.Controller.Operator.OperatorUIController::set_OperatorContacts(Wanker.DCF.DTO.WankerContact)");

            Console.WriteLine(testResult);
            Assert.AreEqual(testResult, "Wanker.DCF.UI.Controller.Operator");

            testResult =
                NfReflect.GetNamespaceWithoutTypeName(
                    ".Controller.Operator.OperatorUIController::set_OperatorContacts(Wanker.DCF.DTO.WankerContact)");

            Console.WriteLine(testResult);
            Assert.AreEqual(testResult, "Controller.Operator");
        }

        [Test]
        public void TestIsClrMethodForProperty()
        {
            var testInput = "get_MyProperty";
            string testOutput;
            var testResult = NfReflect.IsClrMethodForProperty(testInput, out testOutput);

            Assert.IsTrue(testResult);
            Assert.AreEqual("MyProperty",testOutput);

            testInput = "set_MyProperty";
            testResult = NfReflect.IsClrMethodForProperty(testInput, out testOutput);

            Assert.IsTrue(testResult);
            Assert.AreEqual("MyProperty", testOutput);

            testInput = "add_MyProperty";
            testResult = NfReflect.IsClrMethodForProperty(testInput, out testOutput);

            Assert.IsTrue(testResult);
            Assert.AreEqual("MyProperty", testOutput);

            testInput = "remove_MyProperty";
            testResult = NfReflect.IsClrMethodForProperty(testInput, out testOutput);

            Assert.IsTrue(testResult);
            Assert.AreEqual("MyProperty", testOutput);

            testInput = "GetMyValues";
            testResult = NfReflect.IsClrMethodForProperty(testInput, out testOutput);

            Assert.IsFalse(testResult);
            Assert.IsNull(testOutput);
        }
    }

}
