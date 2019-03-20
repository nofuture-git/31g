using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HominiLupusTests
{
    [TestFixture]
    public class ExampleKidnappingTests
    {
        [Test]
        public void ExampleNotKidnapping()
        {
            var testCrime = new Felony
            {
                ActusReus = new Kidnapping
                {
                    IsConfineVictim = lp => lp is JosephAbbyraperEg
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is JosephAbbyraperEg
                }
            };

            var testResult = testCrime.IsValid(new JosephAbbyraperEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }

        [Test]
        public void ExampleKidnappingWrongIntent()
        {
            var testCrime = new Felony
            {
                ActusReus = new Kidnapping
                {
                    IsConfineVictim = lp => lp is JosephAbbyraperEg,
                    IsAsportation = lp => lp is JosephAbbyraperEg
                },
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is JosephAbbyraperEg
                }
            };

            var testResult = testCrime.IsValid(new JosephAbbyraperEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }

        [Test]
        public void ExampleIsKidnapping()
        {
            var testCrime = new Felony
            {
                ActusReus = new Kidnapping
                {
                    IsConfineVictim = lp => lp is JosephAbbyraperEg,
                    IsAsportation = lp => lp is JosephAbbyraperEg
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is JosephAbbyraperEg
                }
            };

            var testResult = testCrime.IsValid(new JosephAbbyraperEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleKidnappingWithConsent()
        {
            var testCrime = new Felony
            {
                ActusReus = new Kidnapping
                {
                    IsConfineVictim = lp => lp is ThomasHitchhikerEg,
                    IsAsportation = lp => lp is ThomasHitchhikerEg,
                    Consent = new VictimConsent
                    {
                        IsCapableThereof = lp => lp is ShawnaHitchinhikenEg,
                        IsApprovalExpressed = lp => false,
                    }
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => false,
                    IsKnowledgeOfWrongdoing = lp => false
                },
                
            };

            var testResult = testCrime.IsValid(new ThomasHitchhikerEg(), new ShawnaHitchinhikenEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class JosephAbbyraperEg : LegalPerson, IDefendant
    {
        public JosephAbbyraperEg() : base("JOSEPH ABBYRAPER") { }
    }

    public class ThomasHitchhikerEg : LegalPerson, IDefendant
    {
        public ThomasHitchhikerEg() : base("THOMAS HITCHHIKER") { }
    }

    public class ShawnaHitchinhikenEg : LegalPerson, IVictim
    {
        public ShawnaHitchinhikenEg() : base ("SHAWNA HITCHINHIKEN") { }
    }
}
