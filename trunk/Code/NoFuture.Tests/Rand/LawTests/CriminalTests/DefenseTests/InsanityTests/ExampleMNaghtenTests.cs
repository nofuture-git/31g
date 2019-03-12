using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense.Excuse.Insanity;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests.InsanityTests
{
    [TestFixture()]
    public class ExampleMNaghtenTests
    {
        [Test]
        public void ExampleMNaghtenFake()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is SusanEg,
                    IsAction = lp => lp is SusanEg
                },
                MensRea = new MaliceAforethought
                {
                    IsIntentOnWrongdoing = lp => lp is SusanEg
                }
            };

            var testResult = testCrime.IsValid(new SusanEg());
            Assert.IsTrue(testResult);

            var testSubject = new MNaghten(testCrime)
            {
                IsMentalDefect = lp => true,
                //by attempting to coverup action, insanity fails
                IsWrongnessOfAware = lp => true 
            };

            testResult = testSubject.IsValid(new SusanEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);

        }

        [Test]
        public void ExampleMNaghtenReal()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is AndreaEg,
                    IsAction = lp => lp is AndreaEg
                },
                MensRea = new MaliceAforethought
                {
                    IsIntentOnWrongdoing = lp => lp is AndreaEg
                }
            };

            var testResult = testCrime.IsValid(new AndreaEg());
            Assert.IsTrue(testResult);

            var testSubject = new MNaghten(testCrime)
            {
                IsMentalDefect = lp => true,
                IsNatureQualityOfAware = lp => false,
                IsWrongnessOfAware = lp => false
            };

            testResult = testSubject.IsValid(new AndreaEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class AndreaEg : LegalPerson, IDefendant
    {
        public AndreaEg() : base("ANDREA SCHIZOPHRENIC") { }
    }

    public class SusanEg : LegalPerson, IDefendant
    {
        public SusanEg() : base("SUSAN INFANTCIDE") {}
    }
}
