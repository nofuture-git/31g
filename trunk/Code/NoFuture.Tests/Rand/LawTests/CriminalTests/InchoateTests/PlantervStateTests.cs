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
    /// Planter v. State, 9 S.W. 3d 156 (1999)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, with solicitation, which is mostly what is said, understanding the lang is critical
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class PlantervStateTests
    {
        [Test]
        public void PlantervState()
        {
            var testCrime = new Felony
            {
                ActusReus = new Solicitation
                {
                    IsInduceAnotherToCrime = lp =>
                    {
                        //defendant want to get paid to make the hit, not pay for it
                        var wasSolicited = Planter.GetSolicitation(lp);
                        return string.Equals(wasSolicited, "you do it");
                    }
                },
                MensRea = new SpecificIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is Planter
                }
            };
            var testResult = testCrime.IsValid(new Planter());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Planter : LegalPerson, IDefendant
    {
        public Planter(): base("WILLIAM EDWARD PLANTER") { }

        public static string GetSolicitation(ILegalPerson lp)
        {
            if (lp is Planter)
                return "I'll do it";
            if (lp is Government)
                return "you do it";
            return null;
        }
    }
}
