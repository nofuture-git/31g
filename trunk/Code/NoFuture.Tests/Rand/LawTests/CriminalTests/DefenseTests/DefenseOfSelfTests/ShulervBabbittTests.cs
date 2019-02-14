﻿using System;
using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Defense;
using NoFuture.Rand.Law.US.Criminal.Defense.Justification;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US.Criminal.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests.DefenseOfSelfTests
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
                MensRea = StrictLiability.Value,
                OtherParties = () => new []{new GrizzlyBear(), }
            };

            var testResult = testCrime.IsValid(new Shuler());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfSelf(testCrime)
            {
                IsReasonableFearOfInjuryOrDeath = lp => true,
                Imminence = new Imminence(testCrime)
                {
                    GetResponseTime = lp => Imminence.NormalReactionTimeToDanger
                },
                Proportionality = new Proportionality<ITermCategory>(testCrime)
                {
                    GetChoice = lp => new DeadlyForce()
                },
                Provacation = new Provacation(testCrime)
                {
                    IsInitiatorOfAttack = lp => lp is GrizzlyBear,
                }
            };

            testResult = testSubject.IsValid(new Shuler());
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
