using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.Contract.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.ConsiderationTests
{
    /// <summary>
    /// QUIGLEY v. WILSON Court of Appeals of Iowa 474 N.W.2d 277 (Iowa Ct. App. 1991)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue: 
    /// [A promise modifying a duty under a contract not fully performed on 
    ///  either side is binding 
    ///     (a) if the modification is fair and equitable in view of circumstances 
    ///         not anticipated by the parties when the contract was made
    /// ]
    /// unanticipated circumstances:
    /// (1) drastic decrease in the value of the land
    /// (2) concern about tax repercussions from reacquiring the land
    /// (3) Wilsons had not received any income from the farm for the previous year
    /// fair and equitable:
    /// (1) negotiations lasting over a period of time
    /// (2) document was written by the seller’s attorney
    /// (3) reduced price was roughly the fair market value of the property
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class QuigleyvWilsonTests
    {
        [Test]
        public void QuigleyvWilson()
        {
            var testSubject = new ComLawContract<Promise>
            {
                Offer = new OfferSellFarmOnContract(),
                Acceptance = o => o is OfferSellFarmOnContract ? new AcceptanceBuyFarmPayInstallments() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => new HashSet<Term<object>> { new Term<object>("the farm", DBNull.Value) }
                }
            };

            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {
                IsSoughtByPromisor = (lp, p) => true,
                IsGivenByPromisee = (lp, p) => true,
                //this is the doctrine point of this one
                IsExistingDuty = p => !(p is OfferSellFarmOnContract || p is AcceptanceBuyFarmPayInstallments)
            };

            var testResult = testSubject.IsValid(new Quigley(), new Wilson());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferSellFarmOnContract : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is Quigley && offeree is Wilson;
        }
    }

    public class AcceptanceBuyFarmPayInstallments : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is Quigley && offeree is Wilson;
        }
    }

    public class Quigley : LegalPerson
    {
        protected Quigley(string name) : base(name) { }
        public Quigley() : base("Lester Quigley, Sr.") { }
    }

    public class Wilson : LegalPerson
    {
        public Wilson() : base("Donald and Janis Wilson") { }
    }

    public class QuigleyCoConservators : Quigley
    {
        public QuigleyCoConservators() : base("Lester L. Quigley, Jr. and Veronna Kay Lovell") { }
    }
}
