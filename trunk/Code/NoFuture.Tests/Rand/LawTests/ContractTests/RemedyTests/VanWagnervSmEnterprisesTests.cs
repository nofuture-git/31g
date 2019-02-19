using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Remedy;
using NoFuture.Rand.Law.Contract.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.RemedyTests
{
    /// <summary>
    /// VAN WAGNER ADVERTISING CORP. v. S&amp;M ENTERPRISES Court of Appeals of New York 67 N.Y.2d 186, 492 N.E.2d 756 (1986)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, specific performance is used to avoid under\over valuing something that cannot be priced
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class VanWagnervSmEnterprisesTests
    {
        [Test]
        public void VanWagnervSmEnterprises()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferAdvertiseOnBuilding(),
                Acceptance = o => o is OfferAdvertiseOnBuilding ? new AcceptanctAdvertiseOnBuilding() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case VanWagner _:
                                return ((VanWagner)lp).GetTerms();
                            case SmEnterprises _:
                                return ((SmEnterprises)lp).GetTerms();
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

            var testResult = testContract.IsValid(new VanWagner(), new SmEnterprises());
            Assert.IsTrue(testResult);

            var testSubject = new SpecificPerformance<Promise>(testContract)
            {
                IsDifficultToProveDmg = lp => lp is VanWagner
            };

            testResult = testSubject.IsValid(new VanWagner(), new SmEnterprises());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferAdvertiseOnBuilding : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is VanWagner || offeror is SmEnterprises)
                   && (offeree is VanWagner || offeree is SmEnterprises);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferAdvertiseOnBuilding;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctAdvertiseOnBuilding : OfferAdvertiseOnBuilding
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctAdvertiseOnBuilding;
            if (o == null)
                return false;
            return true;
        }
    }

    public class VanWagner : LegalPerson
    {
        public VanWagner(): base("VAN WAGNER ADVERTISING CORP.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("Midtown Tunnel exit at East 36th Street in Manhattan", DBNull.Value),
            };
        }
    }

    public class SmEnterprises : LegalPerson
    {
        public SmEnterprises(): base("S&M ENTERPRISES") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("Midtown Tunnel exit at East 36th Street in Manhattan", DBNull.Value),
            };
        }
    }
}
