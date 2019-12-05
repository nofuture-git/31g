using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Procedure.Criminal.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleSearchWarrantTests
    {
        [Test]
        public void TestSearchWarrantIsValid00()
        {
            var testSubject = new SearchWarrant
            {
                GetObjectiveOfSearch = () => new VocaBase("Billy's Closest on 123 Elm St"),
                IsDescribedWithParticularity = r =>
                    string.Equals(r.Name, "Billy's Closest on 123 Elm St", StringComparison.OrdinalIgnoreCase),
                GetIssuerOfWarrant = lps => lps.FirstOrDefault(lp => lp is ExampleJudge),
                IsCapableDetermineProbableCause = lp => lp is ExampleJudge,
                IsNeutralAndDetached = lp => lp is ExampleJudge,
                ProbableCause = new ExampleProbableCause()
            };

            var testResult = testSubject.IsValid(new ExampleSuspect(), new ExampleLawEnforcement(), new ExampleJudge());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class ExampleProbableCause : LegalConcept
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }
    }

    public class ExampleJudge : LegalPerson, IJudge
    {
        public ExampleJudge():base("E.B. Judgin") { }
    }
}
