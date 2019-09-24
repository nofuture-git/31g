using System;
using NUnit.Framework;

namespace NoFuture.Util.Core.Tests
{
    [TestFixture]
    public class NfReflectTests
    {
        [Test]
        public void TestGetTypeNameFromArrayAndGeneric()
        {
            var testResult = NfReflect.GetLastTypeNameFromArrayAndGeneric("int[]");
            Assert.AreEqual("int", testResult);
            testResult = NfReflect.GetLastTypeNameFromArrayAndGeneric("System.Collections.Generic.List`1[System.String]");
            Assert.AreEqual("System.String", testResult);
            testResult =
                NfReflect.GetLastTypeNameFromArrayAndGeneric("System.Tuple`2[System.Int32,System.String]");
            Assert.AreEqual("System.Int32", testResult);

            testResult = NfReflect.GetLastTypeNameFromArrayAndGeneric("System.Tuple`2[System.Collections.Generic.List`1[SomeNamespace.AType],System.String]");
            Assert.AreEqual("SomeNamespace.AType", testResult);

            //test C# style
            testResult =
                NfReflect.GetLastTypeNameFromArrayAndGeneric(
                    "System.Collections.Generic.List<System.String>", "<");
            Assert.AreEqual("System.String", testResult);

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
            Assert.AreEqual("MyType", testResult);

            testResult = NfReflect.GetTypeNameWithoutNamespace("My_SillyName_NoNamespace_Pos_Class");
            Console.WriteLine(testResult);
            Assert.AreEqual("My_SillyName_NoNamespace_Pos_Class", testResult);

            testResult =
                NfReflect.GetNamespaceWithoutTypeName(
                    "Wanker.DCF.UI.Controller.Operator.OperatorUIController::set_OperatorContacts(Wanker.DCF.DTO.WankerContact)");


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
            Assert.AreEqual("Wanker.DCF.UI.Controller.Operator", testResult);

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
            Assert.AreEqual("MyProperty", testOutput);

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

        [Test]
        public void TestGetPropertyValuetype()
        {
            var testContext = new Address();
            var testInput = testContext.GetType().GetProperty("Id");
            var testResult = NfReflect.GetPropertyValueType(testInput);

            Assert.IsNotNull(testResult);
            Assert.AreEqual("System.Int32", testResult.FullName);
            Console.WriteLine(testResult.FullName);
        }

    }
    public class TestDtoLikeType
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int EntityId { get; set; }
        public string AddressId { get; set; }
    }

    public class Entity
    {
        public int Id { get; set; }
        public PersonName Name { get; set; }
        public Address Address { get; set; }
        public Gender? Gender { get; set; }
        public Contact Contact { get; set; }
    }

    public class PersonName
    {
        public string First { get; set; }
        public string Last { get; set; }
        public string Middle { get; set; }

    }

    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public int? Id { get; set; }
    }

    public class Contact
    {
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public enum Gender
    {
        Male = 0,
        Female = 1
    }
}
