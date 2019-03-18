using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.ConsiderationTests
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
            var testSubject = new ComLawContract<Promise>
            {
                Offer = new OfferEmployedAsFishermen(),
                Acceptance = o => o is OfferEmployedAsFishermen ? new AcceptanceOfEmployment() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => new HashSet<Term<object>> { new Term<object>("fishing", DBNull.Value) }
                }
            };

            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {
                IsSoughtByOfferor = (lp, p) => true,
                IsGivenByOfferee = (lp, p) => true,
                IsExistingDuty = p => p is AcceptanceOfEmployment 
            };

            var testResult = testSubject.IsValid(new AlaskaPackersAssoc(), new Domenico());
            Console.WriteLine(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferEmployedAsFishermen : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is AlaskaPackersAssoc && offeree is Domenico;
        }
    }

    public class AcceptanceOfEmployment : Promise
    {
        public decimal Wage { get; set; } = 50m;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is AlaskaPackersAssoc && offeree is Domenico;
        }
    }

    public class AlaskaPackersAssoc : LegalPerson, IOfferor
    {
        public AlaskaPackersAssoc(): base("ALASKA PACKERS’ ASSOCIATION") { }
    }

    public class Domenico : LegalPerson, IOfferee
    {
        public Domenico() : base("DOMENICO") { }
    }
}
