using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Defense.Excuse;
using NoFuture.Law.Criminal.US.Elements.Act;
using NoFuture.Law.Criminal.US.Elements.Intent;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.DefenseTests
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

            var testSubject = new MistakeOfFact
            {
                IsBeliefNegateIntent = lp => lp is Garnett,
                IsStrictLiability = testCrime.MensRea is StrictLiability
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
