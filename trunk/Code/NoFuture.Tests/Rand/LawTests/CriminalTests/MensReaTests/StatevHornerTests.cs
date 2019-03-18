using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.MensReaTests
{
    /// <summary>
    /// State v. Horner, 126 Ohio St.3d 466, 2010-Ohio-3830
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, failure to object to defect (mens rea not said specifically) in indictment constitutes waiver of all but plain error.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class StatevHornerTests
    {
        [Test]
        public void StatevHorner()
        {
            var testSubject = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is Horner,
                    IsVoluntary = lp => lp is Horner
                },
                MensRea = null,
            };

            var testResult = testSubject.IsValid(new Horner());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Horner : LegalPerson, IDefendant
    {
        //public Horner():base("HORNER") { }
    }
}
