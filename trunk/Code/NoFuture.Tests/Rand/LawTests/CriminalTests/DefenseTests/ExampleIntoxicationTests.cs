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
    [TestFixture()]
    public class ExampleIntoxicationTests
    {
        [Test]
        public void ExampleIntoxication()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is DelilahEg,
                    IsVoluntary = lp => lp is DelilahEg,
                },
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is DelilahEg,
                }
            };

            var testResult = testCrime.IsValid(new DelilahEg());
            Assert.IsTrue(testResult);

            var testSubject = new Intoxication
            {
                //the ruffee is taken unknowingly
                IsInvoluntary = lp => lp is DelilahEg,
                IsIntoxicated = lp => lp is DelilahEg
            };

            testResult = testSubject.IsValid(new DelilahEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class DelilahEg : LegalPerson, IDefendant
    {
        public DelilahEg() : base("DELILAH RUFFEE")
        {

        }
    }
}
