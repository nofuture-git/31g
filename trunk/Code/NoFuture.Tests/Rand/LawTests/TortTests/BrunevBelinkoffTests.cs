using System;
using NUnit.Framework;
using NoFuture.Law.US.Persons;
using NoFuture.Law.Tort.US.Terms;
using NoFuture.Law.Tort.US.UnintentionalTort;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.Tests
{
    /// <summary>
    /// Brune v. Belinkoff, 235 N.E.2d 793 (Mass. 1968)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, claiming due care of custom when its not actually a custom
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class BrunevBelinkoffTests
    {
        [Test]
        public void BrunevBelinkoff()
        {
            var test = new Negligence(ExtensionMethods.Tortfeasor)
            {
                SafetyConvention = new EightMgWithDex()
                {
                    //Belinkoff contents among his locality, court denies 
                    IsPracticedByManyAmongGroup = false,
                    IsConformedTo = lp => lp is Belinkoff,
                },
                Causation = new Causation(ExtensionMethods.Tortfeasor)
                {
                    ProximateCause = new ProximateCause(ExtensionMethods.Tortfeasor)
                    {
                        IsForeseeable = lp => true,
                    },
                    FactualCause = new FactualCause(ExtensionMethods.Tortfeasor)
                    {
                        IsButForCaused = lp => lp is Belinkoff
                    }

                }
            };

            var testResult = test.IsValid(new Brune(), new Belinkoff());
            Assert.IsTrue(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class Brune : LegalPerson, IPlaintiff
    {
        public Brune(): base("Brune") { }
    }

    public class Belinkoff : LegalPerson, ITortfeasor
    {
        public Belinkoff(): base("Belinkoff") { }
    }

    public class EightMgWithDex : CustomsTerm
    {

    }
}
