using System;
using NUnit.Framework;
using NoFuture.Law.US.Persons;
using NoFuture.Law.Tort.US.Elements;
using NoFuture.Law.Tort.US.UnintentionalTort;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.Tests
{
    /// <summary>
    /// Zuchowicz v. United States, 140 F.3d 381 (2d Cir. 1998)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, some act increased probability such-and-such and, indeed, such-and-such happened.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ZuchowiczvUnitedStatesTests
    {
        [Test]
        public void ZuchowiczvUnitedStates()
        {
            var test = new Negligence(ExtensionMethods.Tortfeasor)
            {
                Causation = new Causation(ExtensionMethods.Tortfeasor)
                {
                    FactualCause = new StrongCasualConnection(ExtensionMethods.Tortfeasor)
                    {
                        IsIncreasedChancesOfEffect = lp => lp is UnitedStates,
                        IsEffectIndeedPresent = lp => lp is Zuchowicz
                    },
                    ProximateCause = new ProximateCause(ExtensionMethods.Tortfeasor)
                    {
                        IsForeseeable = lp => true
                    }
                }
            };

            var testResult = test.IsValid(new Zuchowicz(), new UnitedStates());
            Assert.IsTrue(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class Zuchowicz : LegalPerson, IPlaintiff
    {
        public Zuchowicz(): base("Zuchowicz") { }
    }

    public class UnitedStates : LegalPerson, ITortfeasor
    {
        public UnitedStates(): base("United States") { }
    }
}
