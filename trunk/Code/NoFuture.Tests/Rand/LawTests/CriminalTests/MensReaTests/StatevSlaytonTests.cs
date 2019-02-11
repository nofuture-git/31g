using System;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.CriminalTests.MensReaTests
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

    public class Slayton : LegalPerson
    {

    }
}
