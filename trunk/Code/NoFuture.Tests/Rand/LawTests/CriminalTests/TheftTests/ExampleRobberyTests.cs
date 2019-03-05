using System;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft;
using NoFuture.Rand.Law.Criminal.US.Elements;
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
                Consent = new Consent
                {
                    IsDenialExpressed = lp => lp is LindseyDealinEg,
                    IsCapableThereof = lp => lp is LindseyDealinEg
                },
                IsTakenPossession = lp => lp is RodneyBlackmailEg,
                IsAsportation = lp => lp is RodneyBlackmailEg,
                IsByViolence = lp => lp is RodneyBlackmailEg,
                SubjectProperty = new LegalProperty("money")
                    {EntitledTo = new LindseyDealinEg(), InPossessionOf = new LindseyDealinEg()},
                PropretyValue = 15000m
            };

            var testResult = testAct.IsValid(new RodneyBlackmailEg(), new LindseyDealinEg());
            Console.WriteLine(testAct.ToString());
            Assert.IsTrue(testResult);
        }
    }
}
