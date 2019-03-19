using System;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.InchoateTests
{
    /// <summary>
    /// State v. Pinson, 895 P.2d 274 (1995)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, two-person crime (buy\sell) if each side has some 
    /// particular name, the charge of solicitation must match the defendant's role in the crime
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class StatevPinsonTests
    {
        [Test]
        public void StatevPinson()
        {
            var testCrime = new Felony
            {
                ActusReus = new Solicitation
                {
                    //Pinson was a buyer and trafficking is for selling
                    IsInduceAnotherToCrime = Pinson.IsSolicitationToTraffic
                },
                MensRea = new SpecificIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is Pinson
                }
            };

            var testResult = testCrime.IsValid(new Pinson());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Pinson : LegalPerson, IDefendant
    {
        public Pinson() : base("JOHNNY PINSON") { }
        public bool IsBuyer { get; set; } = true;

        public static bool IsSolicitationToTraffic(ILegalPerson lp)
        {
            var pinson = lp as Pinson;
            if (pinson == null)
                return false;

            return !pinson.IsBuyer;
        }
    }
}
