using System;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Theft;
using NoFuture.Rand.Law.Criminal.US.Elements.AttendantCircumstances;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.TheftTests
{
    [TestFixture()]
    public class ExampleRobberyTests
    {
        [Test]
        public void ExampleRobberyActTest()
        {
            var testAct = new Robbery
            {
                Consent = new VictimConsent
                {
                    IsApprovalExpressed = lp => false,
                    IsCapableThereof = lp => lp is LindseyDealinEg
                },
                IsTakenPossession = lp => lp is RodneyBlackmailEg,
                IsAsportation = lp => lp is RodneyBlackmailEg,
                IsByViolence = lp => lp is RodneyBlackmailEg,
                SubjectProperty = new LegalProperty("money")
                    {IsEntitledTo = lp => lp is LindseyDealinEg, IsInPossessionOf = lp => lp is LindseyDealinEg, PropertyValue = 15000m },
                
            };

            var testResult = testAct.IsValid(new RodneyBlackmailEg(), new LindseyDealinEg());
            Console.WriteLine(testAct.ToString());
            Assert.IsTrue(testResult);
        }
    }
}
