using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Byrne v. Boadle, 159 Eng. Rep. 299 (Court of Exchequer and Exchequer Chamber, 1863)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, negligence being the very fact that such-and-such happened at all
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ByrnevBoadleTests
    {
        [Test]
        public void ByrnevBoadle()
        {
            var test = new Negligence(ExtensionMethods.Tortfeasor)
            {
                Causation = new Causation(ExtensionMethods.Tortfeasor)
                {
                    FactualCause = new FactualCause(ExtensionMethods.Tortfeasor)
                    {
                        IsButForCaused = lp => lp is Boadle
                    },
                    ProximateCause = new ProximateCause(ExtensionMethods.Tortfeasor)
                    {
                        IsForeseeable = lp => lp.ResIpsaLoquitur()
                    }
                }
            };
            var testResult = test.IsValid(new Byrne(), new Boadle());
            Assert.IsTrue(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class Byrne : LegalPerson, IPlaintiff
    {
        public Byrne(): base("Byrne") { }
    }

    public class Boadle : LegalPerson, ITortfeasor
    {
        public Boadle(): base("Boadle") { }
    }
}
