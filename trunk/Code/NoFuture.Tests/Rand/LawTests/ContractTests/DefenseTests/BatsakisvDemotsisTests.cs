using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Defense.ToPublicPolicy;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.DefenseTests
{
    /// <summary>
    /// BATSAKIS v. DEMOTSIS Court of Civil Appeals of Texas—El Paso 226 S.W.2d 673 (Tex.Civ.App. 1949)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, unconscionable contract will have everything needed to enforce it except for it being just wrong
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class BatsakisvDemotsisTests
    {
        [Test]
        public void BatsakisvDemotsis()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferTwoThousandUsdDuringWar(),
                Acceptance = o => o is OfferTwoThousandUsdDuringWar ? new AcceptTwoThousandUsdDuringWar() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => GetTerms()
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };

            var testSubject = new ByUnconscionability<Promise>(testContract);

            var testResult = testSubject.IsValid(new Batsakis(), new Demotsis());
            //this case is present to present an unconscionable contract
            Assert.IsFalse(testResult);

            Console.WriteLine(testSubject.ToString());
            
        }
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("2000 USD", 2000m),
                new Term<object>("8% per annum", 0.08f),
                new Term<object>("500,000 drachmas", 25m)
            };
        }
    }

    public class OfferTwoThousandUsdDuringWar : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Batsakis || offeror is Demotsis)
                   && (offeree is Batsakis || offeree is Demotsis);
        }
    }

    public class AcceptTwoThousandUsdDuringWar : OfferTwoThousandUsdDuringWar { }

    public class Batsakis : LegalPerson
    {
        public Batsakis():base("Mr. George Batsakis") { }
    }

    public class Demotsis : LegalPerson
    {
        public Demotsis(): base("Eugenia The. Demotsis.") { }
    }
}
