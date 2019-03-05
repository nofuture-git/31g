using System;
using NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HominiLupusTests
{
    /// <summary>
    /// State v. Higgs, 601 N.W.2d 653 (1999)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, body harm is physical pain or injury, illness, or any impairment of physical condition
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class StatevHiggsTests
    {
        [Test]
        public void StatevHiggs()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new Battery
                {
                    IsByViolence = lp => (lp as Higgs)?.IsThrowCupInFace ?? false
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is Higgs
                }
            };

            var testResult = testCrime.IsValid(new Higgs());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Higgs : LegalPerson
    {
        public Higgs() : base("CHARLES DANTE HIGGS") { }
        public bool IsThrowCupInFace { get; set; } = true;
    }

}
