using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.RemedyTests
{
    /// <summary>
    /// HAWKINS v. McGEE Supreme Court of New Hampshire 84 N.H. 114, 146 A. 641 (1929)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class HawkinsvMcGeeTests
    {
        [Test]
        public void HawkinsvMcGee()
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
                            case Hawkins _:
                                return ((Hawkins)lp).GetTerms();
                            case McGee _:
                                return ((McGee)lp).GetTerms();
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
            return (offeror is Hawkins || offeror is McGee)
                   && (offeree is Hawkins || offeree is McGee);
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

    public class Hawkins : LegalPerson
    {
        public Hawkins(): base("GEORGE HAWKINS") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }

    public class McGee : LegalPerson
    {
        public McGee(): base("DR. MCGEE") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }
}
