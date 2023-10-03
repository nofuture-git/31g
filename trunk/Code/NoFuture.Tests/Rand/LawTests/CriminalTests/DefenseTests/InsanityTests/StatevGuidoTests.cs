using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Defense.Excuse.Insanity;
using NoFuture.Law.Criminal.US.Elements.Act;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.DefenseTests.InsanityTests
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

            var testSubject = new MNaghten
            {
                IsMentalDefect = lp => lp is Guido,
                IsWrongnessOfAware = lp => !(lp is Guido),
            };

            testResult = testSubject.IsValid(new Guido());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }
    }

    public class Guido : LegalPerson, IDefendant
    {
        public Guido(): base("ADELE GUIDO") {}
    }
}
