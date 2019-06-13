using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.UnintentionalTort;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// People Express Airlines, Inc. v. Consolidated Rail Corporation, 100 N.J. 246 (1985)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the predicates for econ loss negligence
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class PeopleExpressAirlinesvConsolidatedRailTests
    {
        [Test]
        public void PeopleExpressAirlinesvConsolidatedRail()
        {
            var test = new EconomicLoss(ExtensionMethods.Tortfeasor)
            {
                Causation = new Causation(ExtensionMethods.Tortfeasor)
                {
                    FactualCause = new FactualCause(ExtensionMethods.Tortfeasor)
                    {
                        IsButForCaused = lp => lp is ConsolidatedRailCorporation
                    },
                    ProximateCause = new ProximateCause(ExtensionMethods.Tortfeasor)
                    {
                        IsForeseeable = lp => lp is ConsolidatedRailCorporation,
                        IsDirectCause = lp => lp is ConsolidatedRailCorporation
                    }
                },
                IsEntityTypeIdentifiable = lp => lp is PeopleExpressAirlinesInc,
                IsCountOfEntityPredictable = lp => lp is PeopleExpressAirlinesInc,
                IsEconActivityIdentifiable =  lp => lp is PeopleExpressAirlinesInc,
                IsLocationOfEntityPredictable = lp => lp is PeopleExpressAirlinesInc,
            };
            var testResult = test.IsValid(new PeopleExpressAirlinesInc(), new ConsolidatedRailCorporation());
            Assert.IsTrue(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class PeopleExpressAirlinesInc : LegalPerson, IPlaintiff
    {
        public PeopleExpressAirlinesInc(): base("People Express Airlines, Inc.") { }
    }

    public class ConsolidatedRailCorporation : LegalPerson, ITortfeasor
    {
        public ConsolidatedRailCorporation(): base("Consolidated Rail Corporation") { }
    }
}
