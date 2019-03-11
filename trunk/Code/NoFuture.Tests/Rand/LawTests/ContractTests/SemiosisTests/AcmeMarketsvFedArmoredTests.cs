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
    /// ACME MARKETS, INC. v. FEDERAL ARMORED EXPRESS, INC. Superior Court of Pennsylvania 437 Pa.Super. 41, 648 A.2d 1218 (1994)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, part of knowing if conditional precedent has or has not been met, it must first be known as a conditional precedent
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class AcmeMarketsvFedArmoredTests
    {
        [Test]
        public void AcmeMarketsvFedArmored()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferTransportCashToBank(),
                Acceptance = o => o is OfferTransportCashToBank ? new AcceptanceTransportCashToBank() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case AcmeMarket _:
                                return ((AcmeMarket)lp).GetTerms();
                            case FedArmored _:
                                return ((FedArmored)lp).GetTerms();
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
                IsConditionalTerm = t => t.Name == "receipted",
                IsNotConditionMet = (t, lp) => true
            };

            var testResult = testSubject.IsValid(new AcmeMarket(), new FedArmored());
            Console.WriteLine(testSubject);
            Assert.IsFalse(testResult);
        }
    }

    public class OfferTransportCashToBank : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is AcmeMarket || offeror is FedArmored)
                && (offeree is AcmeMarket || offeree is FedArmored);
        }
    }

    public class AcceptanceTransportCashToBank : OfferTransportCashToBank { }

    public class AcmeMarket : LegalPerson
    {
        public AcmeMarket() : base("ACME MARKETS, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("receipted", DBNull.Value),
                new ContractTerm<object>("accepted",2)
            };
        }
    }

    public class FedArmored : LegalPerson
    {
        public FedArmored() : base("FEDERAL ARMORED EXPRESS, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("receipted", DBNull.Value),
                new ContractTerm<object>("accepted",2)
            };
        }
    }
}
