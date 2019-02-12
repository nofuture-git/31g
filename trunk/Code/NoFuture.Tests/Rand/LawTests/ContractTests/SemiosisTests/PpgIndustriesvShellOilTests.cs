using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Semiosis;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.SemiosisTests
{
    [TestFixture()]
    public class PpgIndustriesvShellOilTests
    {
        [Test]
        public void PpgIndustriesvShellOil()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferSellEthylene(),
                Acceptance = o => o is OfferSellEthylene ? new AcceptanceSellEthylene() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case PpgIndustries _:
                                return ((PpgIndustries)lp).GetTerms();
                            case ShellOil _:
                                return ((ShellOil)lp).GetTerms();
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

            var testSubject = new SyntacticDilemma<Promise>(testContract)
            {
                IsIntendedComposition = terms => terms != null && terms.Any(t => t.Name == "OR"),
            };

            var testResult = testSubject.IsValid(new PpgIndustries(), new ShellOil());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferSellEthylene : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is PpgIndustries || offeror is ShellOil)
                   && (offeree is PpgIndustries || offeree is ShellOil);
        }
    }

    public class AcceptanceSellEthylene : OfferSellEthylene { }

    public class PpgIndustries : LegalPerson
    {
        public PpgIndustries(): base("PPG INDUSTRIES, INC.") {}

        public ISet<Term<object>> GetTerms()
        {
            var trueConst = Expression.Constant(true);
            var falseConst = Expression.Constant(false);
            var xorExpr = Expression.ExclusiveOr(trueConst, falseConst);
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("reasonably beyond its control", 1, new ExpressTerm(), new WrittenTerm()),
                new ContractTerm<object>("XOR", xorExpr, new ExpressTerm(), new WrittenTerm()),
                new ContractTerm<object>("by fire, explosion [...]", 2, new ExpressTerm(), new WrittenTerm())
            };
        }
    }

    public class ShellOil : LegalPerson
    {
        public ShellOil():base("SHELL OIL CO.") {}
        public virtual ISet<Term<object>> GetTerms()
        {
            var trueConst = Expression.Constant(true);
            var falseConst = Expression.Constant(false);
            var orExpr = Expression.Or(trueConst, falseConst);
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("reasonably beyond its control", 1, new ExpressTerm(), new WrittenTerm()),
                new ContractTerm<object>("OR", orExpr, new ExpressTerm(), new WrittenTerm()),
                new ContractTerm<object>("by fire, explosion [...]", 2, new ExpressTerm(), new WrittenTerm())
            };
        }

    }
}
