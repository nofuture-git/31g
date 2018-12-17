using System;
using NUnit.Framework;

namespace NoFuture.Util.NfType.Tests
{
    [TestFixture]
    public class TypeNameTests
    {

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
    }
}
