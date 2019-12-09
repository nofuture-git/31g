using System;
using System.Linq;
using NoFuture.Rand.Law.Procedure.Criminal.US;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleSearchTests
    {
        [Test]
        public void TestSearchIsValid00()
        {
            var testSubject = new Search
            {
                GetConductorOfSearch = lps => lps.FirstOrDefault(lp => lp is ExampleLawEnforcement),
                ExpectationOfPrivacy = new ExpectationOfPrivacy
                {
                    GetSubjectOfSearch = lps => lps.FirstOrDefault(lp => lp is ExamplePersonSearched),
                    IsPrivacyExpected = lp => lp is ExamplePersonSearched,
                    IsPrivacyExpectedReasonable = lp => lp is ExamplePersonSearched
                }
            };

            var testResult = testSubject.IsValid(new ExamplePersonSearched(), new ExampleLawEnforcement());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestSearchIsValid01()
        {
            var testSubject = new Search
            {
                GetConductorOfSearch = lps => lps.FirstOrDefault(lp => lp is ExampleLawEnforcement),
                ExpectationOfPrivacy = new ExpectationOfPrivacy
                {
                    GetSubjectOfSearch = lps => lps.FirstOrDefault(lp => lp is ExamplePersonSearched),
                    Consent = Consent.IsGiven(),
                    IsPrivacyExpected = lp => false,
                    IsPrivacyExpectedReasonable = lp => false
                }
            };

            var testResult = testSubject.IsValid(new ExamplePersonSearched(), new ExampleLawEnforcement());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class ExamplePersonSearched : LegalPerson, IDefendant
    {
        public ExamplePersonSearched() : base("I.B. Ingsearched") { }
    }
}
