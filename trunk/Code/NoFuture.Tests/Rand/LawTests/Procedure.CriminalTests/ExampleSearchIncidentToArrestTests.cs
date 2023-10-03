using System;
using System.Linq;
using NoFuture.Law.Procedure.Criminal.US;
using NoFuture.Law.Procedure.Criminal.US.Intrusions;
using NoFuture.Law.Procedure.Criminal.US.Searches;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleSearchIncidentToArrestTests
    {
        [Test]
        public void TestSearchIncidentToArrestIsValid00()
        {
            var testSubject = new Search
            {
               
                ExpectationOfPrivacy = new SearchIncidentToArrest
                {
                    Arrest = new Arrest
                    {
                        IsAwareOfBeingArrested = lp => true,
                        ProbableCause = new ExampleProbableCause(),
                        IsOccurInPublicPlace = lp => true
                    },
                    IsSearchArrestedPerson = lp => lp is ExampleSuspect,
                    GetSubjectOfSearch = lps => lps.FirstOrDefault(lp => lp is ExampleSuspect)
                },
                InstrumentOfTheState = new InstrumentOfTheState
                {
                    IsAgentOfTheState = lp => true
                },
                GetConductorOfSearch = lps => lps.FirstOrDefault(lp => lp is ExampleLawEnforcement),
                
            };

            var testResult = testSubject.IsValid(new ExampleLawEnforcement(), new ExampleSuspect());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestSearchIncidentToArrestIsValid01()
        {
            var testSubject = new Search
            {

                ExpectationOfPrivacy = new SearchIncidentToArrestMotorVehicle()
                {
                    Arrest = new Arrest
                    {
                        IsAwareOfBeingArrested = lp => true,
                        ProbableCause = new ExampleProbableCause(),
                        IsOccurInPublicPlace = lp => true
                    },
                    IsArresteeNearPassengerCompartment = lp => lp is ExampleLawEnforcement,
                    IsArresteeUnsecured = lp => lp is ExampleLawEnforcement,
                    GetSubjectOfSearch = lps => lps.FirstOrDefault(lp => lp is ExampleSuspect),
                },
                InstrumentOfTheState = new InstrumentOfTheState
                {
                    IsAgentOfTheState = lp => true
                },
                GetConductorOfSearch = lps => lps.FirstOrDefault(lp => lp is ExampleLawEnforcement),

            };

            var testResult = testSubject.IsValid(new ExampleLawEnforcement(), new ExampleSuspect());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestSearchIncidentToArrestIsValid02()
        {
            var testSubject = new Search
            {

                ExpectationOfPrivacy = new SearchIncidentToArrestMotorVehicle()
                {
                    Arrest = new Arrest
                    {
                        IsAwareOfBeingArrested = lp => true,
                        ProbableCause = new ExampleProbableCause(),
                        IsOccurInPublicPlace = lp => true
                    },
                    IsBeliefEvidenceToCrimeIsPresent = lp => lp is ExampleLawEnforcement,
                    GetSubjectOfSearch = lps => lps.FirstOrDefault(lp => lp is ExampleSuspect)
                },
                InstrumentOfTheState = new InstrumentOfTheState
                {
                    IsAgentOfTheState = lp => true
                },
                GetConductorOfSearch = lps => lps.FirstOrDefault(lp => lp is ExampleLawEnforcement),

            };

            var testResult = testSubject.IsValid(new ExampleLawEnforcement(), new ExampleSuspect());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }
}
