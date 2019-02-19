using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Remedy.MoneyDmg;
using NoFuture.Rand.Law.Contract.US.Terms;
using NoFuture.Rand.Law.Contract.US.Ucc;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.RemedyTests
{
    /// <summary>
    /// BRANDEIS MACHINERY & SUPPLY CO., LLC v.CAPITOL CRANE RENTAL, INC. Court of Appeals of Indiana, Fifth District 765 N.E.2d 173 (Ind.Ct.App. 2002)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, more parts to Calc of Loss to injured with UCC
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class BrandeisvCapitolCraneTests
    {
        [Test]
        public void BrandeisvCapitolCrane()
        {
            var testContract = new UccContract<Goods>
            {
                Offer = new OfferPurchaseCrane(),
                Acceptance = o => o is OfferPurchaseCrane ? new AcceptanctPurchaseCrane() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Brandeis _:
                                return ((Brandeis)lp).GetTerms();
                            case CapitolCrane _:
                                return ((CapitolCrane)lp).GetTerms();
                            default:
                                return null;
                        }
                    }
                }
            };

            var testResult = testContract.IsValid(new Brandeis(), new CapitolCrane());
            Assert.IsTrue(testResult);
            var testSubject = new UccExpectation(testContract)
            {
                UccOrigContractPrice = lp =>
                {
                    var blp = lp as Brandeis;
                    if(blp == null)
                        return 0m;
                    return blp.OrigPurchasePrice;
                },
                UccMarketPrice = lp =>
                {
                    var blp = lp as Brandeis;
                    if (blp == null)
                        return 0m;
                    return blp.DepreciationMedianValue;
                },
                CalcLossOther = lp =>
                {
                    var blp = lp as Brandeis;
                    if (blp == null)
                        return 0m;
                    return blp.CostInspectionAndRepair;
                }
            };
            testResult = testSubject.IsValid(new Brandeis(), new CapitolCrane());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferPurchaseCrane : Goods
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Brandeis || offeror is CapitolCrane)
                   && (offeree is Brandeis || offeree is CapitolCrane);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferPurchaseCrane;
            if (o == null)
                return false;
            return true;
        }

        
    }

    public class AcceptanctPurchaseCrane : OfferPurchaseCrane
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctPurchaseCrane;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Brandeis : LegalPerson
    {
        public Brandeis(): base("BRANDEIS MACHINERY & SUPPLY CO., LLC") { }

        public decimal OrigPurchasePrice => 291773.46m;
        public decimal AccumulatedSvcCharge => 159302.38m;
        public decimal CostInspectionAndRepair => 9794.86m;
        public decimal Total => OrigPurchasePrice + AccumulatedSvcCharge + CostInspectionAndRepair;
        public decimal DepreciationMedianValue => 272500m;

        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("crane", DBNull.Value),
            };
        }
    }

    public class CapitolCrane : LegalPerson
    {
        public CapitolCrane(): base("CAPITOL CRANE RENTAL, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("crane", DBNull.Value),
            };
        }

        
    }
}
