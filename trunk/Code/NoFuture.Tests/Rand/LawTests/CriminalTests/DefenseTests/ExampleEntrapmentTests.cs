using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Defense.Excuse;
using NoFuture.Law.Criminal.US.Elements.Act;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.DefenseTests
{
    [TestFixture]
    public class ExampleEntrapmentTests
    {
        [Test]
        public void ExampleEntrapment()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is WinifredEg,
                    IsVoluntary = lp => lp is WinifredEg,
                },
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is WinifredEg,
                }
            };

            var testResult = testCrime.IsValid(new WinifredEg());
            Assert.IsTrue(testResult);

            var testSubject = new Entrapment(testCrime)
            {
                GetOriginatorOfIntent = rea => new ExampleLawEnforcement(),
                IsPredisposedToParticularIntent = lp => !(lp is WinifredEg)
            };

            testResult = testSubject.IsValid(new WinifredEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleEntrapment01()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is WinifredEg,
                    IsVoluntary = lp => lp is WinifredEg,
                },
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is WinifredEg,
                }
            };

            var testResult = testCrime.IsValid(new WinifredEg());
            Assert.IsTrue(testResult);

            var testSubject = new Entrapment(testCrime)
            {
                GetOriginatorOfIntent = rea => new WinifredEg(),
                IsPredisposedToParticularIntent = lp => !(lp is WinifredEg)
            };

            testResult = testSubject.IsValid(new WinifredEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class ExampleLawEnforcement : LegalPerson, ILawEnforcement
    {
        public ExampleLawEnforcement() : base("Johnny Law") { }
    }

    public class WinifredEg : LegalPerson, IDefendant
    {
        public WinifredEg() : base("WINIFRED NA") { }
    }
}
