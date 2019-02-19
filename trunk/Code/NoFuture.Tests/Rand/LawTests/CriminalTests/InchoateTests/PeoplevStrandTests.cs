using System;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.InchoateTests
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

    public class Strand : LegalPerson
    {
        
    }
}
