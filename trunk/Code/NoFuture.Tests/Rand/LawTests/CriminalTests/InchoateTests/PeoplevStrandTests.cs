using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.Inchoate;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.InchoateTests
{
    /// <summary>
    /// People v. Strand, 539 N.W.2d 739 (1995)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, one does not commit an assault intending to attempt to commit a crime
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class PeoplevStrandTests
    {
        [Test]
        public void PeoplevStrand()
        {
            var testCrime = new Felony
            {
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is Strand
                },
                ActusReus = new Attempt
                {
                }
            };

            var testResult = testCrime.IsValid(new Strand());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Strand : LegalPerson, IDefendant
    {
        
    }
}
