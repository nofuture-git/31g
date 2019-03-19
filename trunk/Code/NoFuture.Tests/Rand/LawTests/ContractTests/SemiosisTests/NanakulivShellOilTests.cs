using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Semiosis;
using NoFuture.Rand.Law.Contract.US.Terms;
using NoFuture.Rand.Law.Contract.US.Ucc;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.SemiosisTests
{
    /// <summary>
    /// NANAKULI PAVING AND ROCK CO. v. SHELL OIL CO. United States Court of Appeals for the Ninth Circuit 664 F.2d 772 (9th Cir. 1981)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, with UCC inclusion of implied terms from trade usage, it may produce some exotic and very complex definitions
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class NanakulivShellOilTests
    {
        [Test]
        public void NanakulivShellOil()
        {
            var testContract = new UccContract<Goods>
            {
                Offer = new OfferSellBitumen(),
                Acceptance = o => o is OfferSellBitumen ? new AcceptancePurchaseBitumen() : null,
                Assent = new Agreement
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Nanakuli _:
                                return ((Nanakuli)lp).GetTerms();
                            case ShellOil2 _:
                                return ((ShellOil2)lp).GetTerms();
                            default:
                                return null;
                        }
                    }
                }
            };

            var testResult = testContract.IsValid(new Nanakuli(), new ShellOil2());
            Assert.IsTrue(testResult);
            var testSubject = new SemanticDilemma<Goods>(testContract)
            {
                IsIntendedMeaningAtTheTime = t => t.RefersTo is PriceProtection
            };

            testResult = testSubject.IsValid(new Nanakuli(), new ShellOil2());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferSellBitumen : Goods
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Nanakuli || offeror is ShellOil2)
                && (offeree is Nanakuli || offeree is ShellOil2);
        }
    }

    public class AcceptancePurchaseBitumen : OfferSellBitumen
    {
    }

    public class Nanakuli : LegalPerson, IOfferor
    {
        public Nanakuli() : base("NANAKULI PAVING AND ROCK CO.") { }

        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("posted price", new PriceProtection(), new ExpressTerm(), new WrittenTerm()),
            };
        }
    }

    public class ShellOil2 : ShellOil, IOfferee
    {

        /// <summary>
        /// <![CDATA[
        /// Court concludes that "posted price" actually means: 
        /// at times of price increases, at which times the old 
        /// price is to be charged, for a certain period or for 
        /// a specified tonnage, on work already committed at 
        /// the lower price on nonescalating contracts
        /// ]]>
        /// </summary>
        /// <returns></returns>
        public override ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("posted price", new PriceAsPosted(), new UsageOfTradeTerm(), new ImpliedTerm()),
            };
        }
    }

    public class PriceProtection
    {
        public override bool Equals(object obj)
        {
            return obj is PriceProtection;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode(); 
        }
    }

    public class PriceAsPosted
    {
        public override bool Equals(object obj)
        {
            return obj is PriceAsPosted;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
