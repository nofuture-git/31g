using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense.Excuse;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests
{
    /// <summary>
    /// People v. Register, 60 N.Y.2d 270 (1983)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the intent was recklessness and voluntary intoxication brings such things to an end
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class PeoplevRegisterTests
    {
        [Test]
        public void PeoplevRegister()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is Register,
                    IsVoluntary = lp => lp is Register,
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is Register,
                }
            };

            var testResult = testCrime.IsValid(new Register());
            Assert.IsTrue(testResult);

            var testSubject = new Intoxication(testCrime)
            {
                IsIntoxicated = lp => lp is Register,
                IsVoluntary = lp => lp is Register,
            };

            testResult = testSubject.IsValid(new Register());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Register : LegalPerson, IDefendant
    {
        public Register() : base("BRUCE REGISTER") { }
    }
}
