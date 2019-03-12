using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense.Excuse;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests
{
    /// <summary>
    /// Garnett v. State, 632 A.2d 797 (1993)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, statutory rape is a strict liability crime
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class GarnettvStateTests
    {
        [Test]
        public void GarnettvState()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is Garnett,
                    IsAction = lp => lp is Garnett
                },
                MensRea = StrictLiability.Value
            };

            var testResult = testCrime.IsValid(new Garnett());
            Assert.IsTrue(testResult);

            var testSubject = new MistakeOfFact(testCrime)
            {
                IsBeliefNegateIntent = lp => lp is Garnett
            };

            testResult = testSubject.IsValid(new Garnett());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
            
        }
    }

    public class Garnett : LegalPerson, IDefendant
    {
        public Garnett() : base("LENNARD GARNETT") { }
    }
}
