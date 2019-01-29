using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Breach;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NoFuture.Rand.Law.US.Contracts.Ucc;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.BreachTests
{
    /// <summary>
    /// KCA ELECTRONICS, INC. v. LEGACY ELECTRONICS, INC. Court of Appeal of California, Fourth Appellate District 2007 Cal.App.Unpub.LEXIS 6107 (Ct.App.)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class KcaElectronicsvLegacyElectronicsTests
    {
        [Test]
        public void KcaElectronicsvLegacyElectronics()
        {
            var testContract = new UccContract<Goods>
            {
                Offer = new OfferSellSolderBalls(),
                Acceptance = o => o is OfferSellSolderBalls ? new AcceptanctSellSolderBalls() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case KcaElectronics _:
                                return ((KcaElectronics)lp).GetTerms();
                            case LegacyElectronics _:
                                return ((LegacyElectronics)lp).GetTerms();
                            default:
                                return null;
                        }
                    }
                },
                IsInstallmentContract = true
            };

            var testResult = testContract.IsValid(new KcaElectronics(), new LegacyElectronics());
            Assert.IsTrue(testResult);

            var whatKcaShipped = new OfferSellSolderBalls
            {
                IsCopperExposed = true,
                IsSmashed = true,
                IsUndersized = true,
                IsCoveredWithStickyTapeResidue = true,
                PercentOfDefective = 0.06f
            };

            var testSubject = new PerfectTender<Goods>(testContract)
            {
                ActualPerformance = lp =>
                {
                    if (lp is KcaElectronics)
                        return whatKcaShipped;
                    if (lp is LegacyElectronics)
                        return new AcceptanctSellSolderBalls();
                    return null;
                }
            };

            testResult = testSubject.IsValid(new KcaElectronics(), new LegacyElectronics());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class OfferSellSolderBalls : Goods
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is KcaElectronics || offeror is LegacyElectronics)
                   && (offeree is KcaElectronics || offeree is LegacyElectronics);
        }

        public bool IsCoveredWithStickyTapeResidue { get; set; } = false;
        public bool IsUndersized { get; set; } = false;
        public bool IsSmashed { get; set; } = false;
        public bool IsCopperExposed { get; set; } = false;
        public float PercentOfDefective { get; set; } = 0.0099f;

        public bool IsSubstantialImpairment => PercentOfDefective > 0.01f;

        public override bool Equals(object obj)
        {
            var o = obj as OfferSellSolderBalls;
            if (o == null)
                return false;
            return o.IsCoveredWithStickyTapeResidue == IsCoveredWithStickyTapeResidue
                   && o.IsUndersized == IsUndersized
                   && o.IsSmashed == IsSmashed
                   && o.IsCopperExposed == IsCopperExposed
                ;
        }

        public override bool EquivalentTo(object obj)
        {
            var o = obj as OfferSellSolderBalls;
            if (o == null)
                return false;
            return o.IsSubstantialImpairment == IsSubstantialImpairment;
        }
    }

    public class AcceptanctSellSolderBalls : OfferSellSolderBalls
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctSellSolderBalls;
            if (o == null)
                return false;
            return true;
        }
    }

    public class KcaElectronics : Merchant
    {
        public KcaElectronics(): base("KCA ELECTRONICS, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("solder balls", DBNull.Value),
            };
        }

        public override bool IsSkilledOrKnowledgeableOf(Goods goods)
        {
            return true;
        }
    }

    public class LegacyElectronics : Merchant
    {
        public LegacyElectronics(): base("LEGACY ELECTRONICS, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("solder balls", DBNull.Value),
            };
        }

        public override bool IsSkilledOrKnowledgeableOf(Goods goods)
        {
            return true;
        }
    }
}
