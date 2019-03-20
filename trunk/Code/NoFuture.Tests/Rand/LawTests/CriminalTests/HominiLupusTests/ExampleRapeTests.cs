using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HominiLupusTests
{
    [TestFixture]
    public class ExampleRapeTests
    {
        [Test]
        public void ExampleRape()
        {
            var testCrime = new Felony
            {
                ActusReus = new Rape
                {
                    IsByThreatOfViolence = lp => lp is AlexGamerEg,
                    IsSexualIntercourse = lp => lp is AlexGamerEg, 
                    IsOneOfTwo = lp => lp is AlexGamerEg,
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is AlexGamerEg
                }
            };

            var testResult = testCrime.IsValid(new AlexGamerEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestLackOfConsent()
        {
            var testCrime = new Felony
            {
                ActusReus = new Rape
                {
                    IsSexualIntercourse = lp => lp is AlexGamerEg,
                    IsOneOfTwo = lp => lp is AlexGamerEg,
                    Consent = new VictimConsent
                    {
                        IsApprovalExpressed = lp => false,
                    }
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is AlexGamerEg
                }
            };

            var testResult = testCrime.IsValid(new AlexGamerEg(), new BrandySisterEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestStatutoryRape()
        {
            var testCrime = new Felony
            {
                ActusReus = new Rape
                {
                    IsSexualIntercourse = lp => lp is AlexGamerEg,
                    IsOneOfTwo = lp => lp is AlexGamerEg,
                    Consent = new VictimConsent
                    {
                        IsCapableThereof = lp => ((lp as BrandySisterEg)?.Age  ?? 18) >= 16,
                    }
                },
                MensRea = new StrictLiability()
            };

            var testResult = testCrime.IsValid(new AlexGamerEg(), new BrandySisterEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestConsentDefense()
        {
            var testCrime = new Felony
            {
                ActusReus = new Rape
                {
                    IsSexualIntercourse = lp => lp is AlexGamerEg,
                    IsOneOfTwo = lp => lp is AlexGamerEg,
                    Consent = new VictimConsent
                    {
                        //assented 
                        IsApprovalExpressed = lp => true,
                        IsCapableThereof = lp => ((lp as BrandySisterEg)?.Age ?? 18) >= 16,
                    }
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is AlexGamerEg
                }
            };

            var testResult = testCrime.IsValid(new AlexGamerEg(), new BrandySisterEg(){Age = 21});
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class AlexGamerEg : LegalPerson, IDefendant
    {
        public AlexGamerEg() : base("ALEX GAMER") { }
    }

    public class BradAlsogamerEg : LegalPerson
    {
        public BradAlsogamerEg(): base("BRAD ALSOGAMER") { }
    }

    public class BrandySisterEg : LegalPerson, IVictim
    {
        public BrandySisterEg() : base("BRANDY SISTER") { }

        public int Age { get; set; } = 14;
    }
}
