using System;
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
                IsDomicile = lp => lp is ExampleDefendant00
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff00(), new ExampleDefendant00());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class ExamplePlaintiff00 : LegalPerson, IPlaintiff
    {
        public ExamplePlaintiff00() : base("Example P. Lantiff") { }
    }

    public class ExampleDefendant00 : LegalPerson, IDefendant
    {
        public ExampleDefendant00() : base("Example D. Fendant") {  }
    }
}
