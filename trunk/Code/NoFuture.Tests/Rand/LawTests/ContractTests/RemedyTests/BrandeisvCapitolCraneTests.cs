using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.BreachTests
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class BrandeisvCapitolCraneTests
    {
        [Test]
        public void BrandeisvCapitolCrane()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new Offer_RenameMe(),
                Acceptance = o => o is Offer_RenameMe ? new Acceptanct_RenameMe() : null,
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

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };
        }
    }

    public class Offer_RenameMe : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Brandeis || offeror is CapitolCrane)
                   && (offeree is Brandeis || offeree is CapitolCrane);
        }

        public override bool Equals(object obj)
        {
            var o = obj as Offer_RenameMe;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Acceptanct_RenameMe : Offer_RenameMe
    {
        public override bool Equals(object obj)
        {
            var o = obj as Acceptanct_RenameMe;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Brandeis : LegalPerson
    {
        public Brandeis(): base("") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }

    public class CapitolCrane : LegalPerson
    {
        public CapitolCrane(): base("") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }
}
