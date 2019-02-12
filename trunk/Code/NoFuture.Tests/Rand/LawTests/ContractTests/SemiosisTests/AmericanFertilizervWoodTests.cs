using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NoFuture.Rand.Law.US.Contracts.Ucc;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.SemiosisTests
{
    /// <summary>
    /// AMERICAN FERTILIZER SPECIALISTS, INC. v. WOOD Supreme Court of Oklahoma 1981 OK 116, 635 P.2d 592
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, implied terms as UCC 2-315 implied warrenty
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class AmericanFertilizervWoodTests
    {
        [Test]
        public void AmericanFertilizervWood()
        {
            var testContract = new UccContract<Goods>
            {
                Offer = new Triple19Fertilizer(),
                Acceptance = o => o is Triple19Fertilizer ? new Triple19Fertilizer() : null,
                Assent = new Agreement
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case AmericanFertilizer _:
                                return ((AmericanFertilizer)lp).GetTerms();
                            case Wood2 _:
                                return ((Wood2)lp).GetTerms();
                            default:
                                return null;
                        }
                    }
                }
            };

            var testResult = testContract.IsValid(new AmericanFertilizer(), new Wood2());
            Console.WriteLine(testContract.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Fertilizer : Goods
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is AmericanFertilizer || offeror is Wood2)
                && (offeree is AmericanFertilizer || offeree is Wood2)
                && base.IsValid(offeror, offeree);
        }
    }

    public class Triple19Fertilizer : Fertilizer
    {
        public Triple19Fertilizer() : base()
        {
            Merchantability.IsBuyerRelyingOnSellerJudgement = true;
            Merchantability.IsFit4ParticularPurpose = false;

        }
    }

    public class AmericanFertilizer : Merchant
    {
        public AmericanFertilizer() : base("AMERICAN FERTILIZER SPECIALISTS, INC.") { }
        public virtual ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", 1),
            };
        }
        public override bool IsSkilledOrKnowledgeableOf(Goods goods)
        {
            return goods is Fertilizer;
        }
    }

    public class Wood2 : Wood
    {
        public Wood2() : base("WOOD") { }

        public override ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", 1),
            };
        }
    }


}
