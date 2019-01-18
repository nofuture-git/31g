using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Defense.ToAssent;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.DefenseTests
{
    /// <summary>
    /// STANDARD FINANCE CO. v. ELLIS Intermediate Court of Appeals of Hawaii 3 Haw.App. 614, 657 P.2d 1056 (Haw.Ct.App. 1983)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class StandardFinancevEllisTests
    {
        [Test]
        public void StandardFinancevEllis()
        {
            var testContract = new BilateralContract
            {
                Offer = new OfferOnPromissoryNote(),
                Acceptance = o => o is OfferOnPromissoryNote ? new AcceptOnPromissoryNote() : null,
                MutualAssent = new MutualAssent
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

            var testSubject = new ByDuress<Promise>(testContract);

        }
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("promissory note", DBNull.Value)
            };
        }
    }

    public class OfferOnPromissoryNote : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is StandardFinance || offeror is Ellis)
                   && (offeree is StandardFinance || offeree is Ellis);
        }

        public decimal Amount => 2800m;
        public DateTime OnDate => new DateTime(1976,9,30);
    }

    public class AcceptOnPromissoryNote : OfferOnPromissoryNote { }

    public class StandardFinance : LegalPerson
    {
        public StandardFinance() : base("Standard Finance Company, Limited") { }
    }

    public class Ellis : LegalPerson
    {
        public Ellis() : base("Betty Ellis") { }
    }
}
