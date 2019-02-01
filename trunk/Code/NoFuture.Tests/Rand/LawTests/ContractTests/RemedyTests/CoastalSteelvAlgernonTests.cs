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
    public class CoastalSteelvAlgernonTests
    {
        [Test]
        public void CoastalSteelvAlgernon()
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
                            case CoastalSteel _:
                                return ((CoastalSteel)lp).GetTerms();
                            case Algernon _:
                                return ((Algernon)lp).GetTerms();
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
            return (offeror is CoastalSteel || offeror is Algernon)
                   && (offeree is CoastalSteel || offeree is Algernon);
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

    public class CoastalSteel : LegalPerson
    {
        public CoastalSteel(): base("") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }

    public class Algernon : LegalPerson
    {
        public Algernon(): base("") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }
}
