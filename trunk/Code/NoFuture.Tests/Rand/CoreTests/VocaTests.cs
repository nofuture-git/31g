﻿using System;
using System.Linq;
using Newtonsoft.Json;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.CoreTests
{
    [TestFixture]
    public class VocaTests
    {
        [Test]
        public void TestUpsertName()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            testSubject.AddName(KindsOfNames.Legal, "OtherName");
            var testResult = testSubject.Names.FirstOrDefault(x => x.Item1 == KindsOfNames.Legal);

            Assert.IsNotNull(testResult);
            Assert.AreEqual("OtherName", testResult.Item2);

            //add a bitwise kind of name
            testSubject.AddName(KindsOfNames.Legal | KindsOfNames.Technical, "TechnicalName");

            //assert the original is not effected
            testResult = testSubject.Names.FirstOrDefault(x => x.Item1 == KindsOfNames.Legal);
            Assert.IsNotNull(testResult);
            Assert.AreEqual("OtherName", testResult.Item2);

            testResult = testSubject.Names.FirstOrDefault(x => x.Item1 == (KindsOfNames.Legal | KindsOfNames.Technical));
            Assert.IsNotNull(testResult);
            Assert.AreEqual("TechnicalName", testResult.Item2);
        }

        [Test]
        public void TestGetName()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            Assert.AreEqual("TestCorporation", testSubject.GetName(KindsOfNames.Legal));
        }

        [Test]
        public void TestAnyOfKindOfName()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Former | KindsOfNames.Technical, "TechnicalName"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            Assert.IsTrue(testSubject.AnyNames(k => k == KindsOfNames.Legal));
            Assert.IsTrue(testSubject.AnyNames(k => k == (KindsOfNames.Former | KindsOfNames.Technical)));

            Assert.IsFalse(testSubject.AnyNames(k => k == (KindsOfNames.Legal | KindsOfNames.Technical)));
        }


        [Test]
        public void TestAnyOfNameAs()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Former | KindsOfNames.Technical, "TechnicalName"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            Assert.IsTrue(testSubject.AnyNames(n => n == "TechnicalName"));

            Assert.IsFalse(testSubject.AnyNames(n => n == ""));
            Assert.IsFalse(testSubject.AnyNames(n => n == "foijhdlkjae"));

        }

        [Test]
        public void TestAnyOfKindAndValue()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Former | KindsOfNames.Technical, "TechnicalName"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            Assert.IsTrue(testSubject.AnyNames((k,v) => k == KindsOfNames.Legal && v == "TestCorporation"));
            Assert.IsTrue(testSubject.AnyNames((k,v) => k == (KindsOfNames.Former | KindsOfNames.Technical) && v == "TechnicalName"));

            Assert.IsFalse(testSubject.AnyNames((k, v) => k == KindsOfNames.Legal && v == "Test Corporation"));

            Assert.IsFalse(testSubject.AnyNames((k, v) => k == KindsOfNames.Former && v == "TestCorporation"));

        }

        [Test]
        public void TestRemoveNameByKind()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal | KindsOfNames.Technical, "TechnicalName"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            testSubject.RemoveName(k => k == KindsOfNames.Legal);
            var testResult =
                testSubject.Names.FirstOrDefault(x => x.Item1 == KindsOfNames.Legal);
            Assert.IsNull(testResult);
            testResult =
                testSubject.Names.FirstOrDefault(x => x.Item1 == (KindsOfNames.Legal | KindsOfNames.Technical));
            Assert.IsNotNull(testResult);
        }

        [Test]
        public void TestRemoveNameByValue()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal | KindsOfNames.Technical, "TestCorporation"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            var testResult = testSubject.RemoveName(n => n =="TestCorporation");
            Assert.AreEqual(2, testResult);

            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal | KindsOfNames.Technical, "TestCorporation"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            testResult = testSubject.RemoveName(n => n =="TechnicalName");
            Assert.AreEqual(0, testResult);
        }

        [Test]
        public void TestRemoveNameByKindAndValue()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal | KindsOfNames.Technical, "TestCorporation"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            var testResult = testSubject.RemoveName((k,v) => k == (KindsOfNames.Legal | KindsOfNames.Technical) && v == "TestCorporation");
            Assert.IsTrue(testResult > 0);
            var testEntry = testSubject.Names.FirstOrDefault(x => x.Item1 == KindsOfNames.Legal);
            Assert.IsNotNull(testEntry);
            Assert.AreEqual("TestCorporation", testEntry.Item2);

        }

        [Test]
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

        [Test]
        public void TestToToDiscreteKindsOfNames()
        {
            var testResult = (KindsOfNames.Legal | KindsOfNames.Technical).ToDiscreteKindsOfNames();
            Assert.IsNotNull(testResult);
            var tra = testResult.ToArray();
            Assert.AreEqual(2, tra.Length);
            Assert.IsTrue(tra.Contains(KindsOfNames.Legal));
            Assert.IsTrue(tra.Contains(KindsOfNames.Technical));

        }

        [Test]
        public void TestAnyOfKindContaining()
        {
            var testSubject = new VocaBase();
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Former | KindsOfNames.Technical | KindsOfNames.Legal, "TechnicalName"));
            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, "TestCorporation"));

            Assert.IsTrue(testSubject.AnyNames(k => k.ToDiscreteKindsOfNames().Any(v => v ==KindsOfNames.Former)));
            Assert.IsTrue(testSubject.AnyNames(k => k.ToDiscreteKindsOfNames().Any(v => v == KindsOfNames.Technical)));
            Assert.IsTrue(testSubject.AnyNames(k => k.ToDiscreteKindsOfNames().Any(v => v == KindsOfNames.Legal)));

            Assert.IsFalse(testSubject.AnyNames(k => k == KindsOfNames.Former));
            Assert.IsFalse(testSubject.AnyNames(k => k == KindsOfNames.Technical));


            testSubject.Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Abbrev | KindsOfNames.Group | KindsOfNames.Colloquial, "TestCorporation"));

            Assert.IsTrue(testSubject.AnyNames(k => k.ToDiscreteKindsOfNames().Any(v => v == KindsOfNames.Abbrev)));
            Assert.IsTrue(testSubject.AnyNames(k => k.ToDiscreteKindsOfNames().Any(v => v == KindsOfNames.Group)));
            Assert.IsTrue(testSubject.AnyNames(k => k.ToDiscreteKindsOfNames().Any(v => v == KindsOfNames.Colloquial)));
        }

        [Test]
        public void TestToData()
        {

            var testSubject = new VocaBase();
            testSubject.AddName(KindsOfNames.Maiden, "Butler");
            testSubject.AddName(KindsOfNames.First, "Judith");
            testSubject.AddName(KindsOfNames.Surname, "Williamson");
            testSubject.AddName(KindsOfNames.Colloquial | KindsOfNames.First, "Judy");
            testSubject.AddName(KindsOfNames.Former | KindsOfNames.Surname | KindsOfNames.Spouse, "Cricket");

            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);

            var asJson = JsonConvert.SerializeObject(testResult, Formatting.Indented);
            Console.WriteLine(asJson);
        }
    }
}
