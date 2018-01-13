using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Tests
{
    [TestClass]
    public class VocaTests
    {
        [TestMethod]
        public void TestUpsertName()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            testSubject.UpsertName(KindsOfNames.Legal, "OtherName");
            var testResult = testSubject.Names.FirstOrDefault(x => x.Item1 == KindsOfNames.Legal);

            Assert.IsNotNull(testResult);
            Assert.AreEqual("OtherName", testResult.Item2);

            //add a bitwise kind of name
            testSubject.UpsertName(KindsOfNames.Legal | KindsOfNames.Technical, "TechnicalName");

            //assert the original is not effected
            testResult = testSubject.Names.FirstOrDefault(x => x.Item1 == KindsOfNames.Legal);
            Assert.IsNotNull(testResult);
            Assert.AreEqual("OtherName", testResult.Item2);

            testResult = testSubject.Names.FirstOrDefault(x => x.Item1 == (KindsOfNames.Legal | KindsOfNames.Technical));
            Assert.IsNotNull(testResult);
            Assert.AreEqual("TechnicalName", testResult.Item2);
        }

        [TestMethod]
        public void TestGetName()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            Assert.AreEqual("TestCorporation", testSubject.GetName(KindsOfNames.Legal));
        }

        [TestMethod]
        public void TestAnyOfKindOfName()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Former | KindsOfNames.Technical, "TechnicalName"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            Assert.IsTrue(testSubject.AnyOfKindOfName(KindsOfNames.Legal));
            Assert.IsTrue(testSubject.AnyOfKindOfName(KindsOfNames.Former | KindsOfNames.Technical));

            Assert.IsFalse(testSubject.AnyOfKindOfName(KindsOfNames.Legal | KindsOfNames.Technical));
        }


        [TestMethod]
        public void TestAnyOfNameAs()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Former | KindsOfNames.Technical, "TechnicalName"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            Assert.IsTrue(testSubject.AnyOfNameAs("TechnicalName"));

            Assert.IsFalse(testSubject.AnyOfNameAs(null));
            Assert.IsFalse(testSubject.AnyOfNameAs(""));
            Assert.IsFalse(testSubject.AnyOfNameAs("foijhdlkjae"));

        }

        [TestMethod]
        public void TestAnyOfKindAndValue()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Former | KindsOfNames.Technical, "TechnicalName"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            Assert.IsTrue(testSubject.AnyOfKindAndValue(KindsOfNames.Legal, "TestCorporation"));
            Assert.IsTrue(testSubject.AnyOfKindAndValue(KindsOfNames.Former | KindsOfNames.Technical, "TechnicalName"));

            Assert.IsFalse(testSubject.AnyOfKindAndValue(KindsOfNames.Legal, "Test Corporation"));

            Assert.IsFalse(testSubject.AnyOfKindAndValue(KindsOfNames.Former, "TestCorporation"));

        }

        [TestMethod]
        public void TestRemoveNameByKind()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal | KindsOfNames.Technical, "TechnicalName"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            testSubject.RemoveNameByKind(KindsOfNames.Legal);
            var testResult =
                testSubject.Names.FirstOrDefault(x => x.Item1 == KindsOfNames.Legal);
            Assert.IsNull(testResult);
            testResult =
                testSubject.Names.FirstOrDefault(x => x.Item1 == (KindsOfNames.Legal | KindsOfNames.Technical));
            Assert.IsNotNull(testResult);
        }

        [TestMethod]
        public void TestRemoveNameByValue()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal | KindsOfNames.Technical, "TestCorporation"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            var testResult = testSubject.RemoveNameByValue("TestCorporation");
            Assert.AreEqual(2, testResult);

            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal | KindsOfNames.Technical, "TestCorporation"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            testResult = testSubject.RemoveNameByValue("TechnicalName");
            Assert.AreEqual(0, testResult);
        }

        [TestMethod]
        public void TestRemoveNameByKindAndValue()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal | KindsOfNames.Technical, "TestCorporation"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            var testResult = testSubject.RemoveNameByKindAndValue(KindsOfNames.Legal | KindsOfNames.Technical, "TestCorporation");
            Assert.IsTrue(testResult);
            var testEntry = testSubject.Names.FirstOrDefault(x => x.Item1 == KindsOfNames.Legal);
            Assert.IsNotNull(testEntry);
            Assert.AreEqual("TestCorporation", testEntry.Item2);

        }

        [TestMethod]
        public void TestEquals()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal | KindsOfNames.Technical, "TestCorporation"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            var testSubject2 = new VocaBase();
            testSubject2.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal | KindsOfNames.Technical, "TestCorporation"));
            testSubject2.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            Assert.IsTrue(testSubject.Equals(testSubject2));

            testSubject2.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Former, "Machina"));

            Assert.IsFalse(testSubject.Equals(testSubject2));
        }

    }
}
