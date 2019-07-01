using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.Property.US;
using NoFuture.Rand.Law.Tort.US.IntentionalTort;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Hinman v. Pacific Air Transport, 84 F.2d 755 (9th Cir. 1936)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, passing over or under land outside of the useable range is not trespass
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class HinmanvPacificAirTransportTests
    {
        [Test]
        public void HinmanvPacificAirTransport()
        {
            var test = new TrespassToLand(ExtensionMethods.Tortfeasor)
            {
                IsTangibleEntry = lp => false,
                IsIntangibleEntry = lp => false,
                Injury = new NoDamage(),
                SubjectProperty = new LegalProperty("some land"),
                Causation = new Causation(ExtensionMethods.Tortfeasor)
                {
                    FactualCause = new FactualCause(ExtensionMethods.Tortfeasor)
                    {
                        IsButForCaused = lp => lp is PacificAirTransport
                    },
                    ProximateCause = new ProximateCause(ExtensionMethods.Tortfeasor)
                    {
                        IsDirectCause = lp => lp is PacificAirTransport,
                        IsForeseeable = lp => lp is PacificAirTransport
                    }
                }
            };

            var testResult = test.IsValid(new Hinman(), new PacificAirTransport());
            Assert.IsFalse(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class Hinman : LegalPerson, IPlaintiff
    {
        public Hinman(): base("Hinman") { }
    }

    public class PacificAirTransport : LegalPerson, ITortfeasor
    {
        public PacificAirTransport(): base("Pacific Air Transport") { }
    }
}
