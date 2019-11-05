using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Courts;
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
            var testSubject = new PersonalJurisdiction(new StateCourt("CA"))
            {
                GetDomicileLocation = lp => lp is ExampleDefendant ? new VocaBase("CA") : null,
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        [Test]
        public void TestIsValidWithConsent()
        {
            const string MA = "Massachusetts";
            const string GA = "Georgia";

            var testSubject = new PersonalJurisdiction(new StateCourt(MA))
            {
                Consent = Consent.NotGiven() ,
                GetDomicileLocation = lp => lp is ExampleDefendant ? new VocaBase(GA) : new VocaBase(MA),
                GetCurrentLocation = lp => lp is ExampleDefendant ? new VocaBase(GA) : new VocaBase(MA),
                GetInjuryLocation = lp => lp is ExamplePlaintiff ? new VocaBase(MA) : null
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());

            testSubject.Consent = Consent.IsGiven();
            testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        [Test]
        public void TestBothLiveSameState()
        {
            const string MA = "Massachusetts";
            const string GA = "Georgia";

            var testSubject = new PersonalJurisdiction(new StateCourt(MA))
            {
                GetDomicileLocation = lp => new VocaBase(MA),
                GetInjuryLocation = lp => new VocaBase(GA)
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
