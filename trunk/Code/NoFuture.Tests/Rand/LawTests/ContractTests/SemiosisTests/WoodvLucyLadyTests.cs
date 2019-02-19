using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.SemiosisTests
{
    /// <summary>
    /// WOOD v. LUCY, LADY DUFF-GORDON Court of Appeals of New York 222 N.Y. 88, 118 N.E. 214 (1917)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the balancing part of consideration may itself be implied
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class WoodvLucyLadyTests
    {
        [Test]
        public void WoodvLucyLady()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferTurnThisVogueIntoMoney(),
                Acceptance = o => o is OfferTurnThisVogueIntoMoney ? new AcceptanceTurnThisVogueIntoMoney() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Wood _:
                                return ((Wood)lp).GetTerms();
                            case LucyLady _:
                                return ((LucyLady)lp).GetTerms();
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

            var testResult = testContract.IsValid(new Wood(), new LucyLady());
            Console.WriteLine(testContract.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferTurnThisVogueIntoMoney : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Wood || offeror is LucyLady)
                && (offeree is Wood || offeree is LucyLady);
        }
    }

    public class AcceptanceTurnThisVogueIntoMoney : OfferTurnThisVogueIntoMoney { }

    public class Wood : LegalPerson
    {
        public Wood() : base("OTIS F. WOOD") { }
        public Wood(string name) : base(name) { }

        public virtual ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("exclusive right", DBNull.Value),
                new ContractTerm<object>("reasonable efforts to bring profits and revenues into existence", 2, new ImpliedTerm()),
            };
        }
    }

    public class LucyLady : LegalPerson
    {
        public LucyLady() : base("LUCY, LADY DUFF-GORDON") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("exclusive right", DBNull.Value, new ExpressTerm(), new WrittenTerm()),
                new ContractTerm<object>("reasonable efforts to bring profits and revenues into existence", 2, new ImpliedTerm()),
            };
        }
    }
}
