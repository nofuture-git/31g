﻿using System;
using System.Linq;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Criminal;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.CriminalTests
{
    [TestFixture]
    public class CrimeTests
    {

        [Test]
        public void TestCompareTo()
        {
            ICrime[] testSubjects =
            {
                new Misdemeanor(), new Infraction(),
                new Misdemeanor(), new Felony(),
                new Infraction(), new Felony(),
                new Infraction(), new Misdemeanor(),
            };

            Array.Sort(testSubjects);

            foreach(var c in testSubjects)
                Console.WriteLine(c.GetType().Name);
            Assert.IsTrue(testSubjects.First() is Infraction);
            Assert.IsTrue(testSubjects.Last() is Felony);
        }

        [Test]
        public void TestGetDefendant()
        {
            var testResult = Government.GetDefendant(new TestPerson00(), null, null);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult.Name);
        }
    }

    public class TestPerson00 : LegalPerson
    {
        public TestPerson00() : base("TEST PERSON")  { }
    }
}