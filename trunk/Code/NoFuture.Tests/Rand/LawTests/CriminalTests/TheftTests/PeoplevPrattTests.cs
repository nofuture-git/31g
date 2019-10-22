using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Theft;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.TheftTests
{
    /// <summary>
    /// People v. Pratt, 656 N.W.2d 866 (2002)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, stolen means taking witout permission, intenting to return it doesn't make it anything less
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class PeoplevPrattTests
    {
        [Test]
        public void PeoplevPratt()
        {
            var testCrime = new Felony
            {
                ActusReus = new ByReceiving
                {
                    
                    IsTakenPossession = lp => lp is Pratt,
                    SubjectProperty = new LegalProperty("1990 Buick Regal")
                    {
                        IsEntitledTo = lp => lp is PrattFormerGirlfriend,
                        IsInPossessionOf = lp => lp is PrattFormerGirlfriend,
                        PropertyValue = dt => 1000.99m
                    },
                    //court finds its stolen, by statute, regardless of intent-to-return
                    IsPresentStolen = lp => lp is Pratt
                }
            };

            var testResult = testCrime.IsValid(new Pratt(), new PrattFormerGirlfriend());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Pratt : LegalPerson, IDefendant
    {
        public Pratt(): base("EDDIE JUNIOR PRATT") { }
    }

    public class PrattFormerGirlfriend : LegalPerson, IVictim
    {
        public PrattFormerGirlfriend(): base("FORMER GIRLFRIEND") {  }
    }
}
