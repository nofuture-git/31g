using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Semiosis;
using NoFuture.Rand.Law.Contract.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.SemiosisTests
{
    /// <summary>
    /// TOWN OF FAIRFIELD v. D’ADDARIO Supreme Court of Connecticut 149 Conn. 358, 179 A.2d 826 (1961)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, implied terms are read in especially timely notice
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class TownFairfieldvDaddarioTests
    {
        [Test]
        public void TownFairfieldvDaddario()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferConstructSewerageSystem(),
                Acceptance = o => o is OfferConstructSewerageSystem ? new AcceptanceConstructSewerageSystem() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case TownFairfield _:
                                return ((TownFairfield)lp).GetTerms();
                            case Daddario _:
                                return ((Daddario)lp).GetTerms();
                            default:
                                return null;
                        }
                    }
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByOfferee = (lp, p) => true,
                IsSoughtByOfferor = (lp, p) => true
            };

            var testSubject = new ConditionsPrecedent<Promise>(testContract)
            {
                IsConditionalTerm = t => t.Name == "notice",
                IsNotConditionMet = (t, lp) =>
                {
                    if (t.Name == "notice" && lp is TownFairfield)
                        return false;
                    return true;
                }
            };

            var testResult = testSubject.IsValid(new TownFairfield(), new Daddario());
            Console.WriteLine(testResult.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class OfferConstructSewerageSystem : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is TownFairfield || offeror is Daddario)
                && (offeree is TownFairfield || offeree is Daddario);
        }
    }

    public class AcceptanceConstructSewerageSystem : OfferConstructSewerageSystem { }

    public class TownFairfield : LegalPerson, IOfferor
    {
        public TownFairfield() : base("TOWN OF FAIRFIELD") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("sewerage system", DBNull.Value),
                new ContractTerm<object>("indemnify", 2),
                //this is the term the court reads in - Town of Fairfield took 3 years to notify
                new ContractTerm<object>("notice", 3, new ImpliedTerm())
            };
        }
    }

    public class Daddario : LegalPerson, IOfferee
    {
        public Daddario() : base("F. FRANCIS D’ADDARIO") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("sewerage system", DBNull.Value),
                new ContractTerm<object>("indemnify", 2),
                new ContractTerm<object>("notice", 3, new ImpliedTerm())
            };
        }
    }
}
