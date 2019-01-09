using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.ConsiderationTests
{
    /// <summary>
    /// ALASKA PACKERS’ ASSOCIATION v. DOMENICO Circuit Court of Appeals, Ninth Circuit 117 F. 99 (1902)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, there is not consideration in a contract where 
    /// either the promise or return-promise is an existing duty
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class AlaskaPackersAssocvDomenicoTests
    {
        [Test]
        public void AlaskaPackersAssocvDomenico()
        {
            var testSubject = new BilateralContract
            {
                Offer = new OfferEmployedAsFishermen(),
                Acceptance = o => o is OfferEmployedAsFishermen ? new AcceptanceOfEmployment() : null,
                MutualAssent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => new HashSet<Term<object>> { new Term<object>("fishing", DBNull.Value) }
                }
            };

            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {
                IsSoughtByPromisor = (lp, p) => true,
                IsGivenByPromisee = (lp, p) => true,
                IsExistingDuty = p => p is AcceptanceOfEmployment 
            };

            var testResult = testSubject.IsValid(new AlaskaPackersAssoc(), new Domenico());
            Console.WriteLine(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferEmployedAsFishermen : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is AlaskaPackersAssoc && offeree is Domenico;
        }
    }

    public class AcceptanceOfEmployment : Promise
    {
        public decimal Wage { get; set; } = 50m;

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is AlaskaPackersAssoc && offeree is Domenico;
        }
    }

    public class AlaskaPackersAssoc : VocaBase, ILegalPerson
    {
        public AlaskaPackersAssoc(): base("ALASKA PACKERS’ ASSOCIATION") { }
    }

    public class Domenico : VocaBase, ILegalPerson
    {
        public Domenico() : base("DOMENICO") { }
    }
}
