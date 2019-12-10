using System;
using System.Linq;
using NoFuture.Rand.Law.Procedure.Criminal.US;
using NoFuture.Rand.Law.Procedure.Criminal.US.Searches;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleMobileVehicleSearchTests
    {
        [Test]
        public void TestMobileVehicleSearchIsValid00()
        {
            IMobileVehicleSearch testSubject = new MobileVehicleSearch
            {
                GetConductorOfSearch = lps => lps.FirstOrDefault(lp => lp is ExampleLawEnforcement),
                IsBeliefEvidenceToCrimeIsPresent = lp => lp is ExampleLawEnforcement
            };

            var testResult = testSubject.IsValid(new ExampleSuspect(), new ExampleLawEnforcement());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }
}
