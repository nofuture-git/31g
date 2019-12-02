using System;
using System.Linq;
using NoFuture.Rand.Law.Procedure.Criminal.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class InstrumentOfTheStateTests
    {
        [Test]
        public void TestInstrumentOfTheStateIsValid00()
        {
            var testSubject = new InstrumentOfTheState
            {
                GetConductorOfSearch = lps => lps.FirstOrDefault(lp => lp is ExampleCitizenSearchConductor),
                IsAcquiescenceOfTheState = lp => false,
                IsPromoteInterestOfTheState = lp => false
            };
            var testResult = testSubject.IsValid(new ExampleCitizenSearchConductor(), new ExampleLawEnforcement());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        [Test]
        public void TestInstrumentOfTheStateIsValid01()
        {
            var testSubject = new InstrumentOfTheState
            {
                GetConductorOfSearch = lps => lps.FirstOrDefault(lp => lp is ExampleCitizenSearchConductor),
                IsAcquiescenceOfTheState = lp => true,
                IsPromoteInterestOfTheState = lp => true
            };
            var testResult = testSubject.IsValid(new ExampleCitizenSearchConductor(), new ExampleLawEnforcement());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class ExampleCitizenSearchConductor : LegalPerson
    {
        public ExampleCitizenSearchConductor() : base("U.B. Searchin") { }
    }

    public class ExampleLawEnforcement : LegalPerson, ILawEnforcement
    {
        public ExampleLawEnforcement() : base("Johnny Law") { }
    }
}
