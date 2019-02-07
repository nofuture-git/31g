using System;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Elements;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.CriminalTests.MensReaTests
{
    /// <summary>
    /// HAROLD E. STAPLES, III, PETITIONER v. UNITED STATES No. 92-1441
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class HaroldStaplesvUsTests
    {
        [Test]
        public void HaroldStaplesvUs()
        {
            var testSubject = new Felony
            {
                IsChargedWith = lp =>
                {
                    var hs = lp as HaroldStaples;
                    return (hs?.IsPossessionOfAr15Rifle ?? false) && (hs.IsAr15RifleFullyAutoFire);
                }
            };

            testSubject.ActusReus.IsAction = lp => lp is HaroldStaples;
            testSubject.ActusReus.IsVoluntary = lp => lp is HaroldStaples;

            //mens rea is not needed for Fed Statute intended to regulate
            //for example ownership of hand gernades was a crime without mens rea
            testSubject.MensRea = null;
            var testResult = testSubject.IsValid(new HaroldStaples());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

            //for this case, gun ownership is not the same thing
            testSubject.MensRea = new MensRea();
            testResult = testSubject.IsValid(new HaroldStaples());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class HaroldStaples : LegalPerson
    {
        public HaroldStaples(): base("HAROLD E. STAPLES") { }
        public bool IsPossessionOfAr15Rifle => true;
        public bool IsAr15RifleFullyAutoFire => true;
    }
}
