using System;
using NoFuture.Rand.Law.Property.US;
using NoFuture.Rand.Law.Tort.US.Defense;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.IntentionalTort;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Vincent v. Lake Erie Transp. Co., 109 Minn. 456, 124 N.W. 221 (1910)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, necessity defense is a incomplete privilege since you are responsible for damage caused during trespass
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class VincentvLakeErieTranspCoTests
    {
        [Test]
        public void VincentvLakeErieTranspCo()
        {
            var theBoat = new TheSteamshipReynolds();
            var theDock = new DockInDuluth();

            var tresspass = new TrespassToLand()
            {
                Causation = new Causation(ExtensionMethods.Tortfeasor)
                {
                    FactualCause = new FactualCause(ExtensionMethods.Tortfeasor)
                    {
                        IsButForCaused = lp => lp is Vincent
                    },
                    ProximateCause = new ProximateCause(ExtensionMethods.Tortfeasor)
                    {
                        IsForeseeable = lp => lp is Vincent
                    }
                },
                SubjectProperty = theDock,
                IsTangibleEntry = lp => lp is Vincent,
                PropertyDamage = new Damage(ExtensionMethods.Tortfeasor)
                {
                    SubjectProperty = theDock,
                    ToValue = pr => pr is DockInDuluth
                },
            };

            //assert that a trespass has occured
            var testResult = tresspass.IsValid(new Vincent(), new LakeErieTranspCo());
            Assert.IsTrue(testResult);
            Console.WriteLine(tresspass.ToString());

            var testDefense = new NecessityPrivilege<ITermCategory>(ExtensionMethods.Tortfeasor)
            {
                IsMultipleInHarm = lp => true,
                IsPublicInterest = lp => false,
                IsResponsibleForSituationArise = lp => false,
                Proportionality = new ChoiceThereof<ITermCategory>(ExtensionMethods.Tortfeasor)
                {
                    GetChoice = lp => new NoDamage(),
                    GetOtherPossibleChoices = lp => new []{new Damage()}
                },
            };

            //w/o trespass assigned, no way to know damage so its a valid defense
            testResult = testDefense.IsValid(new Vincent(), new LakeErieTranspCo());
            Assert.IsTrue(testResult);
            Console.WriteLine(testDefense.ToString());

            testDefense.ClearReasons();
            testDefense.Trespass = tresspass;
            testResult = testDefense.IsValid(new Vincent(), new LakeErieTranspCo());
            Assert.IsFalse(testResult);
            Console.WriteLine(testDefense.ToString());
        }
    }

    public class NoDamage : Damage
    {
        public override int GetRank()
        {
            return base.GetRank() - 1;
        }
    }

    public class TheSteamshipReynolds : PersonalProperty
    {

    }

    public class DockInDuluth : RealProperty
    {

    }

    public class Vincent : LegalPerson, ITortfeasor
    {
        public Vincent(): base("Vincent") { }
    }

    public class LakeErieTranspCo : LegalPerson, IPlaintiff
    {
        public LakeErieTranspCo(): base("Lake Erie Transp. Co.,") { }
    }
}
