using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Semiosis;
using NoFuture.Rand.Law.Contract.US.Terms;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.SemiosisTests
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
                IsGivenByOfferee = (lp, p) => true,
                IsSoughtByOfferor = (lp, p) => true
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
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is PpgIndustries || offeror is ShellOil)
                   && (offeree is PpgIndustries || offeree is ShellOil);
        }
    }

    public class AcceptanceSellEthylene : OfferSellEthylene { }

    public class PpgIndustries : LegalPerson, IOfferor
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

    public class ShellOil : LegalPerson, IOfferee
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
