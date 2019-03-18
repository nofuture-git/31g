using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.MensReaTests
{
    /// <summary>
    /// State v. Slayton, 154 P.3d 1057 (2007)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, strict liability 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class StatevSlaytonTests
    {
        [Test]
        public void StatevSlayton()
        {
            var testSubject = new Misdemeanor
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is Slayton,
                    IsVoluntary = lp => lp is Slayton
                },
                MensRea = StrictLiability.Value
            };

            var testResult = testSubject.IsValid(new Slayton());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Slayton : LegalPerson, IDefendant
    {

    }
}
