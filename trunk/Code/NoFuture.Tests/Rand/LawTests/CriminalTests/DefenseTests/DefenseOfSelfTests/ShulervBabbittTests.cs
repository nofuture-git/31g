using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests.DefenseOfSelfTests
{
    /// <summary>
    /// Shuler v. Babbitt, 49 F.Supp.2d 1165 (1998)
    /// (this is a civil case)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, self-defense concerning a wild animal attack
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ShulervBabbittTests
    {
        [Test]
        public void ShulervBabbitt()
        {
            var testCrime = new Infraction
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is Shuler,
                    IsAction = lp => lp is Shuler,
                },
                MensRea = StrictLiability.Value
            };

            var testResult = testCrime.IsValid(new Shuler());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfSelf(testCrime)
            {
                IsReasonableFearOfInjuryOrDeath = lp => true,
                Imminence = new Imminence
                {
                    GetResponseTime = lp => Imminence.NormalReactionTimeToDanger
                },
                Proportionality = new Proportionality<ITermCategory>()
                {
                    GetChoice = lp => new DeadlyForce()
                },
                Provacation = new Provacation()
                {
                    IsInitiatorOfAttack = lp => lp is GrizzlyBear,
                }
            };

            testResult = testSubject.IsValid(new Shuler(), new GrizzlyBear());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class GrizzlyBear : LegalPerson
    {
        
    }

    public class Shuler : LegalPerson
    {
        public Shuler() : base("JOHN E. SHULER") { }
    }

    public class Babbitt : LegalPerson
    {
        public Babbitt():base("BRUCE BABBITT, Secretary, Department of Interior") { }
    }
}
