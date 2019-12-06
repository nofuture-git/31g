using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Procedure.Criminal.US;
using NoFuture.Rand.Law.Procedure.Criminal.US.Intrusions;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Criminal.Tests
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
    }
}
