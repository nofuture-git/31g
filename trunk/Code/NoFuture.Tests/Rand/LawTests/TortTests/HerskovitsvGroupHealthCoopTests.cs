using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.UnintentionalTort;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Herskovits v. Group Health Coop., 664 P.2d 474 (Wash. 1983)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, illustrate the lost-chance approach
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class HerskovitsvGroupHealthCoopTests
    {
        [Test]
        public void HerskovitsvGroupHealthCoop()
        {
            var test = new LostChanceApproach(ExtensionMethods.Tortfeasor)
            {
                FactualCause = new FactualCause(ExtensionMethods.Tortfeasor)
                {
                    IsButForCaused = lp => lp is GroupHealthCoop,
                },
                GetLostProbability = lp => lp is Herskovits ? 0.39D : 1D,
                GetActualProbability = lp => lp is Herskovits ? 0.25D : 1D
            };

            var testResult = test.IsValid(new Herskovits(), new GroupHealthCoop());
            Console.WriteLine(test.ToString());
            Assert.IsTrue(testResult);

        }
    }

    public class Herskovits : LegalPerson, IPlaintiff
    {
        public Herskovits(): base("Herskovits") { }
    }

    public class GroupHealthCoop : LegalPerson, ITortfeasor
    {
        public GroupHealthCoop(): base("Group Health Coop.") { }
    }
}
