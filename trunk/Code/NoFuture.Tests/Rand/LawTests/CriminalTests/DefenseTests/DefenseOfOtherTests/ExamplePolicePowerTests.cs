using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests.DefenseOfOtherTests
{
    [TestFixture]
    public class ExamplePolicePowerTests
    {
        [Test]
        public void ExamplePolicePower()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is OfficerColinEg,
                    IsAction = lp => lp is OfficerColinEg
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is OfficerColinEg,
                    IsIntentOnWrongdoing = lp => lp is OfficerColinEg
                }
            };
            var testResult = testCrime.IsValid(new OfficerColinEg());
            Assert.IsTrue(testResult);

            var testSubject = new PolicePower(testCrime)
            {
                IsAgentOfTheState = lp => lp is OfficerColinEg,
                //example has officer shooting out window drive-by stlye on a fleeing person
                IsReasonableUseOfForce = lp => false
            };

            testResult = testSubject.IsValid(new OfficerColinEg(), new LindaEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class OfficerColinEg : LegalPerson, IDefendant
    {
        public OfficerColinEg() : base("OFFICER COLIN") { }
    }

    public class LindaEg : LegalPerson, IVictim
    {
        public LindaEg() : base("LINDA BRATHEIF") {}
    }
}
