using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.OffersTests
{
    /// <summary>
    /// EVER-TITE ROOFING CORP. v. GREEN Court of Appeal of Louisiana, Second Circuit 83 So. 2d 449 (1955)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// This is again a sequence issue.  The speciality with this case is Acceptance was affirmed as soon 
    /// as Ever-Tite started the job.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class EverTiteRoofingCorpvGreenTests
    {
        [Test]
        public void EverTiteRoofingCorpvGreen()
        {
            var testSubject = new ComLawContract<Promise>
            {
                Offer = new OfferReRoofResidence(),
                Acceptance = o => o is OfferReRoofResidence ? new AcceptanceViaCredit() : null,
                Assent = new MutualAssent
                {
                    TermsOfAgreement = lp => GetTerms,
                    IsApprovalExpressed = lp =>
                    {
                        var everTite = lp as EverTiteRoofingCorp;
                        if (everTite != null)
                            return everTite.IsApprovalExpressed();
                        var green = lp as Green;
                        if (green != null)
                            return green.IsApprovalExpressed(new EverTiteRoofingCorp
                            {
                                IsTruckLoaded = true,
                                IsOnProperty = true
                            });
                        return false;
                    }
                }
            };

            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {
                IsSoughtByOfferor = (lp, promise) => lp is EverTiteRoofingCorp && promise is AcceptanceViaCredit,
                IsGivenByOfferee = (lp, promise) => lp is Green && promise is OfferReRoofResidence
            };

            var testResult = testSubject.IsValid(new EverTiteRoofingCorp(), new Green());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        private static object _term00 = new object();
        private static object _term01 = new object();
        public static ISet<Term<object>> GetTerms => new HashSet<Term<object>>
        {
            new Term<object>("re-roofing their residence", _term00),
            new Term<object>("Webster Parish, Louisiana", _term01)

        };
    }

    public class OfferReRoofResidence : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is EverTiteRoofingCorp || offeree is EverTiteRoofingCorp;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class AcceptanceViaCredit : Promise
    {
        public bool IsApprovedByLendingAgy { get; set; } = true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is Green || offeree is Green && IsApprovedByLendingAgy;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class EverTiteRoofingCorp : LegalPerson, IOfferor
    {
        public EverTiteRoofingCorp() : base("EVER-TITE ROOFING CORP.") { }

        public bool IsTruckLoaded { get; set; }
        public bool IsOnProperty { get; set; }
        public bool IsActivelyWorking { get; set; }

        public bool IsApprovalExpressed()
        {
            return true;
        }
    }

    public class Green : LegalPerson, IOfferee
    {
        public Green() : base("G. T. Green and Mrs. Jessie Fay Green") { }

        public bool IsApprovalExpressed(EverTiteRoofingCorp contractor)
        {
            //the court ruled that:  acceptance = start-of-work and 
            // start-of-work was as soon as loading truck began 
            return contractor.IsTruckLoaded;
        }
    }
}
