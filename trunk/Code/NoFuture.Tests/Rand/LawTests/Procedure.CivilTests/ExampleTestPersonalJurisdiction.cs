﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests
{
    [TestFixture]
    public class ExampleTestPersonalJurisdiction
    {
        [Test]
        public void TestPersonalJurisdictionIsValid()
        {
            var testSubject = new PersonalJurisdiction()
            {
                IsDomicile = lp => lp is ExampleDefendant
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class ExamplePlaintiff : LegalPerson, IPlaintiff
    {
        public ExamplePlaintiff() : base("Example P. Lantiff") { }
    }

    public class ExampleDefendant : LegalPerson, IDefendant
    {
        public ExampleDefendant() : base("Example D. Fendant") {  }
    }
}
