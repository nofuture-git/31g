using System;
using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Defense.Excuse;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests.InsanityTests
{
    /// <summary>
    /// State v. Guido, 191 A.2d 45 (1993)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the mental defect has meaning in its effect, not as a medical label
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class StatevGuidoTests
    {
        [Test]
        public void StatevGuido()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is Guido,
                    IsAction = lp => lp is Guido,
                },
                MensRea = new MaliceAforethought
                {
                    IsKnowledgeOfWrongdoing = lp => lp is Guido,
                }
            };

            var testResult = testCrime.IsValid(new Guido());
            Assert.IsTrue(testResult);

            var testSubject = new MNaghten(testCrime)
            {
                IsMentalDefect = lp => lp is Guido,
                IsWrongnessOfAware = lp => !(lp is Guido),
            };

            testResult = testSubject.IsValid(new Guido());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }
    }

    public class Guido : LegalPerson
    {
        public Guido(): base("ADELE GUIDO") {}
    }
}
