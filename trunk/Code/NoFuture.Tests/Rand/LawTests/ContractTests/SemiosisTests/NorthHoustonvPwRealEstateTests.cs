using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Semiosis;
using NoFuture.Rand.Law.Contract.US.Terms;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.SemiosisTests
{

    /// <summary>
    /// NORTH HOUSTON INTERNATIONAL, L.L.C., v. PW REAL ESTATE INVESTMENTS, INC. Court of Appeals of Texas, Fourteenth District—Houston 2003 Tex.App.LEXIS 9185
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, when terms are explicit they are explicit and must be met exactly as stated
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class NorthHoustonvPwRealEstateTests
    {
        [Test]
        public void NorthHoustonvPwRealEstate()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferRefinMortgage(),
                Acceptance = o => o is OfferRefinMortgage ? new AcceptanceRefinMortgage() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case NorthHouston _:
                                return ((NorthHouston)lp).GetTerms();
                            case PwRealEstate _:
                                return ((PwRealEstate)lp).GetTerms();
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
                IsConditionalTerm = t => t.Name == "estoppel certificate",
                IsNotConditionMet = (t, lp) =>
                {
                    var isParty = lp is NorthHouston;
                    if (!isParty)
                        return false;
                    var isTerm = t.Name == "estoppel certificate";
                    if (!isTerm)
                        return false;

                    //they failed to meet this requirement
                    if (isTerm && isParty)
                        return true;
                    return false;
                }
            };

            var testResult = testSubject.IsValid(new NorthHouston(), new PwRealEstate());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class OfferRefinMortgage : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is NorthHouston || offeror is PwRealEstate)
                && (offeree is NorthHouston || offeree is PwRealEstate);
        }
    }

    public class AcceptanceRefinMortgage : OfferRefinMortgage
    {
    }

    public class NorthHouston : LegalPerson, IOfferor
    {
        public NorthHouston() : base("NORTH HOUSTON INTERNATIONAL, L.L.C.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("mortgage", DBNull.Value),
                new ContractTerm<object>("estoppel certificate", 2)
            };
        }
    }

    public class PwRealEstate : LegalPerson, IOfferee
    {
        public PwRealEstate() : base("PW REAL ESTATE INVESTMENTS, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("mortgage", DBNull.Value),
                new ContractTerm<object>("estoppel certificate", 2)
            };
        }
    }
}
