using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Remedy;
using NoFuture.Rand.Law.Contract.US.Terms;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.RemedyTests
{
    /// <summary>
    /// LACLEDE GAS CO. v. AMOCO OIL CO. United States Court of Appeals, Eighth Circuit 522 F.2d 33 (8th Cir. 1975)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, specific performance can be applied when substitution is not available
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class LacledeGasvAmocoOilTests
    {
        [Test]
        public void LacledeGasvAmocoOil()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferSupplyPropane(),
                Acceptance = o => o is OfferSupplyPropane ? new AcceptanctSupplyPropane() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case LacledeGas _:
                                return ((LacledeGas)lp).GetTerms();
                            case AmocoOil _:
                                return ((AmocoOil)lp).GetTerms();
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

            var testResult = testContract.IsValid(new LacledeGas(), new AmocoOil());
            Assert.IsTrue(testResult);

            var testSubject = new SpecificPerformance<Promise>(testContract)
            {
                IsDifficultToSubstitute = lp => lp is LacledeGas
            };

            testResult = testSubject.IsValid(new LacledeGas(), new AmocoOil());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferSupplyPropane : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is LacledeGas || offeror is AmocoOil)
                   && (offeree is LacledeGas || offeree is AmocoOil);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferSupplyPropane;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctSupplyPropane : OfferSupplyPropane
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctSupplyPropane;
            if (o == null)
                return false;
            return true;
        }
    }

    public class LacledeGas : LegalPerson, IOfferor
    {
        public LacledeGas(): base("LACLEDE GAS CO.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("propane", DBNull.Value),
            };
        }
    }

    public class AmocoOil : LegalPerson, IOfferee
    {
        public AmocoOil(): base("AMOCO OIL CO.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("propane", DBNull.Value),
            };
        }
    }
}
