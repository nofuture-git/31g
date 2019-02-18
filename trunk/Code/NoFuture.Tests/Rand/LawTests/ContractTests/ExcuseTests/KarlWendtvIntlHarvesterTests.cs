using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Excuse;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.ExcuseTests
{
    /// <summary>
    /// KARL WENDT FARM EQUIPMENT CO. v. INT’L HARVESTER CO. United States Court of Appeals for the Sixth Circuit 931 F.2d 1112 (6th Cir. 1991)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, impracticability and frustration mean more then just losing money
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class KarlWendtvIntlHarvesterTests
    {
        [Test]
        public void KarlWendtvIntlHarvester()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferFarmEquipDealerAgreement(),
                Acceptance = o => o is OfferFarmEquipDealerAgreement ? new AcceptanctFarmEquipDealerAgreement() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case KarlWendt _:
                                return ((KarlWendt)lp).GetTerms();
                            case IntlHarvester _:
                                return ((IntlHarvester)lp).GetTerms();
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

            var testResult = testContract.IsValid(new KarlWendt(), new IntlHarvester());
            Assert.IsTrue(testResult);

            ExcuseBase<Promise> testSubject = new ImpracticabilityOfPerformance<Promise>(testContract)
            {
                IsAtFault = lp => false,
                IsContraryInForm = lp => false,
                //IH argues that Farm Crisis of 1980s was sufficient to this excuse
                //the court found this false
                IsBasicAssumptionGone = lp => false,
            };

            testResult = testSubject.IsValid(new KarlWendt(), new IntlHarvester());
            Assert.IsFalse(testResult);

            testSubject = new FrustrationOfPurpose<Promise>(testContract)
            {
                IsAtFault = lp => false,
                IsContraryInForm = lp => false,
                IsBasicAssumptionGone = lp => false,
                IsFrustrationSubstantial = lp => false,
                IsPrincipalPurposeFrustrated = lp => false
            };
            testResult = testSubject.IsValid(new KarlWendt(), new IntlHarvester());
            Assert.IsFalse(testResult);

            //test if the conditions had indeed been met
            testSubject = new FrustrationOfPurpose<Promise>(testContract)
            {
                IsAtFault = lp => false,
                IsContraryInForm = lp => false,
                IsBasicAssumptionGone = lp => lp is IntlHarvester,
                IsFrustrationSubstantial = lp => lp is IntlHarvester,
                IsPrincipalPurposeFrustrated = lp => lp is IntlHarvester
            };
            testResult = testSubject.IsValid(new KarlWendt(), new IntlHarvester());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferFarmEquipDealerAgreement : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is KarlWendt || offeror is IntlHarvester)
                   && (offeree is KarlWendt || offeree is IntlHarvester);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferFarmEquipDealerAgreement;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctFarmEquipDealerAgreement : OfferFarmEquipDealerAgreement
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctFarmEquipDealerAgreement;
            if (o == null)
                return false;
            return true;
        }
    }

    public class KarlWendt : LegalPerson
    {
        public KarlWendt(): base("KARL WENDT FARM EQUIPMENT CO.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }

    public class IntlHarvester : LegalPerson
    {
        public IntlHarvester(): base("INT’L HARVESTER CO.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }
}
