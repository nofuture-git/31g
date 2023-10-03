using System;
using NoFuture.Law.Procedure.Civil.US.Pleadings;
using NoFuture.Law.US.Courts;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Civil.Tests
{
    [TestFixture]
    public class ExampleTestContributionClaim
    {
        [Test]
        public void TestContributionClaimIsValid()
        {
            var testSubject = new ContributionClaim()
            {
                GetAssertion = lp => new ExampleCauseForAction(),
                GetRequestedRelief = lp => new ExampleRequestRelief(),
                IsSigned = lp => true,
                IsShareInLiability = lp => lp is ExampleThirdParty,
                Court = new StateCourt("MN")
            };

            var testResult =
                testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant(), new ExampleThirdParty());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }

    }

    

    public class ExampleThirdParty : LegalPerson, IThirdParty
    {
        public ExampleThirdParty() : base("Sir Party III") { }
    }

    public class ExampleCauseForAction : LegalConcept
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }
    }

    public class ExampleRequestRelief : LegalConcept
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
