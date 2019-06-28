using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Theft;
using NoFuture.Rand.Law.Criminal.US.Elements.AttendantCircumstances;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.TheftTests
{
    [TestFixture]
    public class ExampleByTakingTests
    {
        [Test]
        public void ExampleFiveFingerTheft()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new ByTaking
                {
                    SubjectProperty = new ChewingGum(){ PropertyValue = 1.25m },
                    IsTakenPossession = lp => lp is JeremyTheifEg,
                    IsAsportation = lp => lp is JeremyTheifEg
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is JeremyTheifEg
                }
            };
            var testResult = testCrime.IsValid(new JeremyTheifEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleEmbezzlementTheft()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new ByTaking()
                {
                    SubjectProperty =  new TangiblePersonalProperty("payment for gas"),
                    IsTakenPossession = lp => lp is JeremyTheifEg,
                    IsAsportation = lp => lp is JeremyTheifEg
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is JeremyTheifEg
                }
            };
            var testResult = testCrime.IsValid(new JeremyTheifEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleInvalidTheftWhenOwner()
        {
            var jermey = new JeremyTheifEg();
            var property = new ChewingGum {EntitledTo = jermey, PropertyValue = 1.25m };
            var testCrime = new Misdemeanor
            {
                ActusReus = new ByTaking()
                {
                    SubjectProperty = property,
                    IsTakenPossession = lp => lp is JeremyTheifEg,
                    IsAsportation = lp => lp is JeremyTheifEg
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is JeremyTheifEg
                }
            };
            var testResult = testCrime.IsValid(new JeremyTheifEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }

        [Test]
        public void ExampleVictimConsentGiven()
        {
            var cody = new CodyFriendEg();
            var property = new ChewingGum {EntitledTo = cody};

            var testCrime = new Misdemeanor
            {
                ActusReus = new ByTaking()
                {
                    SubjectProperty = property,
                    IsTakenPossession = lp => lp is JeremyTheifEg,
                    IsAsportation = lp => lp is JeremyTheifEg,
                    Consent = new VictimConsent
                    {
                        IsCapableThereof = lp => true,
                        //Cody said it was ok to take the property 
                        IsApprovalExpressed = lp => lp is CodyFriendEg,
                    }
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is JeremyTheifEg
                },
            };
            var testResult = testCrime.IsValid(new JeremyTheifEg(), new CodyFriendEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }

        [Test]
        public void ExampleTreacheryTest()
        {
            var testAttendantCircumstance = new FiduciaryRelationship
            {
                IsTrustBetween = (lp1, lp2) => (lp1 is JeremyTheifEg && lp2 is CodyFriendEg)
                                               || (lp1 is CodyFriendEg && lp2 is JeremyTheifEg)
            };

            var testCrime = new Misdemeanor
            {
                ActusReus = new ByTaking()
                {
                    SubjectProperty = new ChewingGum(){ PropertyValue = 1.25m },
                    IsTakenPossession = lp => lp is JeremyTheifEg,
                    IsAsportation = lp => lp is JeremyTheifEg
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is JeremyTheifEg
                }
            };
            testCrime.AttendantCircumstances.Add(testAttendantCircumstance);
            var testResult = testCrime.IsValid(new JeremyTheifEg(), new CodyFriendEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class ChewingGum : LegalProperty
    {

    }

    public class JeremyTheifEg : LegalPerson, IDefendant
    {
        public JeremyTheifEg() : base("JEREMY THEIF") {}
    }

    public class CodyFriendEg : LegalPerson, IVictim
    {
        public CodyFriendEg() : base("CODY FRIEND") { }
    }
}
