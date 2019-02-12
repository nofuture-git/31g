using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Remedy;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.RemedyTests
{
    /// <summary>
    /// TRUCK RENT-A-CENTER, INC. v. PURITAN FARMS 2nd, INC. Court of Appeals of New York 41 N.Y.2d 420, 393 N.Y.S.2d 365 (1977)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, liquidated damages are fine so long as they are reasonable and not punitive
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class TruckRentvPuritanFarmsTests
    {
        [Test]
        public void TruckRentvPuritanFarms()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferRentTrucks(),
                Acceptance = o => o is OfferRentTrucks ? new AcceptanctRentTrucks() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case TruckRent _:
                                return ((TruckRent)lp).GetTerms();
                            case PuritanFarms _:
                                return ((PuritanFarms)lp).GetTerms();
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

            var testResult = testContract.IsValid(new TruckRent(), new PuritanFarms());
            Assert.IsTrue(testResult);

            var testSubject = new LiquidatedDmg<Promise>(testContract)
            {
                //court found this is reasonable 
                IsDisproportionateToActual = lp => !(lp is TruckRent || lp is PuritanFarms)
            };

            testResult = testSubject.IsValid(new TruckRent(), new PuritanFarms());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferRentTrucks : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is TruckRent || offeror is PuritanFarms)
                   && (offeree is TruckRent || offeree is PuritanFarms);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferRentTrucks;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctRentTrucks : OfferRentTrucks
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctRentTrucks;
            if (o == null)
                return false;
            return true;
        }
    }

    public class TruckRent : LegalPerson
    {
        public TruckRent(): base("TRUCK RENT-A-CENTER, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("truck lease", DBNull.Value),
            };
        }
    }

    public class PuritanFarms : LegalPerson
    {
        public PuritanFarms(): base("PURITAN FARMS 2nd, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("truck lease", DBNull.Value),
            };
        }
    }
}
