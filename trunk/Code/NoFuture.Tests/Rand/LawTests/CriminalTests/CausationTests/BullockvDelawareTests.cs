using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.CausationTests
{
    /// <summary>
    /// Bullock v. State, 775 A.2d. 1043 (2001)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, intervening superseding cause in that the death of a person was partially caused by their own unlawful actions
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class BullockvDelawareTests
    {
        [Test]
        public void BullockvDelaware()
        {
            var testSubject = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is Bullock,
                    IsAction = lp => lp is Bullock
                },
                MensRea = new Recklessly
                {
                    IsDisregardOfRisk = lp =>
                    {
                        lp.AddReasonEntry("reckless causation was nonetheless not established");
                        return false;
                    },
                    IsUnjustifiableRisk = lp => lp is Bullock
                }

            };

            var testResult = testSubject.IsValid(new Bullock());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);

        }
    }

    public class Bullock : LegalPerson, IDefendant
    {

    }
}
