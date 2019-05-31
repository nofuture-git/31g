using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Andrews v. United Airlines, 24 F.3d 39 (9th Cir. 1994)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, enhanced duty for legal person type of "Common Carriers"
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class AndrewsvUnitedAirlinesTests
    {
        [Test]
        public void AndrewsvUnitedAirlines()
        {
            var test = new LearnedHandsFormula(ExtensionMethods.Tortfeasor)
            {
                GetRiskOfLoss = lp =>
                {
                    if (!(lp is UnitedAirlines))
                    {
                        return 0d;
                    }

                    return 135d / 175000d;
                },
                GetCostOfLoss = lp =>
                {
                    if (!(lp is UnitedAirlines))
                    {
                        return 0m;
                    }
                    //just a guess
                    return 1000000m;
                },
                GetCostOfPrecaution = lp =>
                {
                    if (!(lp is UnitedAirlines))
                    {
                        return 0m;
                    }

                    var numberOfAirplanes = 175000d / 365d;
                    var costOfAdditionalEquip = 7000d;

                    return Convert.ToDecimal(numberOfAirplanes * costOfAdditionalEquip);
                }
            };

            var calcCost = test.Calculate(new UnitedAirlines());
            Console.WriteLine(calcCost);
            test.ClearReasons();
            var testResult = test.IsValid(new UnitedAirlines(), new Andrews());
            Assert.IsFalse(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class Andrews : LegalPerson, IPlaintiff
    {
        public Andrews(): base("Andrews") { }
    }

    public class UnitedAirlines : LegalPerson, ITortfeasor
    {
        public UnitedAirlines(): base("United Airlines") { }
    }
}
