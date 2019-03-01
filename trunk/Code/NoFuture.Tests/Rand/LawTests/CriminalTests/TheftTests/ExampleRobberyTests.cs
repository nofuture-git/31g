using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NUnit.Framework;
using NUnit.Framework.Internal;

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
                IsByForce = lp => lp is RodneyBlackmailEg,
                SubjectOfTheft = new LegalProperty("money")
                    {EntitledTo = new LindseyDealinEg(), InPossessionOf = new LindseyDealinEg()},
                AmountOfTheft = 15000m
            };

            var testResult = testAct.IsValid(new RodneyBlackmailEg(), new LindseyDealinEg());
            Console.WriteLine(testAct.ToString());
            Assert.IsTrue(testResult);
        }
    }
}
