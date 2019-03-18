using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.AcceptanceTests
{
    /// <summary>
    /// DAVIS v. JACOBY Supreme Court of California 1 Cal. 2d 370, 34 P.2d 1026 (1934)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine on this is acceptance cannot be understood unless the offer is understood as 
    /// bilateral or unilateral contract.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class DavisvJacobyTests
    {
        [Test]
        public void DavisvJacoby()
        {
            var testSubject = new ComLawContract<Promise>
            {
                Offer = new OfferApr12(),
                Acceptance = o => o is OfferApr12 ? new AcceptanceApr14() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => new HashSet<Term<object>>
                    {
                        new Term<object>("inherit everything", DBNull.Value)
                    }
                },
                
            };
            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {
                IsSoughtByOfferor = (lp, p) => lp is RupertAndBlancheWhitehead && p is AcceptanceApr14,
                IsGivenByOfferee = (lp, p) => lp is FrankAndCaroDavis && p is OfferApr12
            };

            var testResult = testSubject.IsValid(new RupertWhitehead(), new FrankAndCaroDavis());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());

            var testSubjectAsPerformance = new ComLawContract<Performance>
            {
                Offer = new OfferApr12() as Promise,
                Acceptance = o => o is OfferApr12 ? new AcceptanceAccording2TrialCourt() : null
            };
            testSubjectAsPerformance.Consideration = new Consideration<Performance>(testSubjectAsPerformance)
            {
                IsSoughtByOfferor = (lp, p) => lp is RupertAndBlancheWhitehead && p is AcceptanceAccording2TrialCourt,
                IsGivenByOfferee = (lp, p) => lp is FrankAndCaroDavis && p is OfferApr12
            };

            testResult = testSubjectAsPerformance.IsValid(new RupertWhitehead(), new FrankAndCaroDavis());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubjectAsPerformance.ToString());

        }
    }

    public class OfferApr12 : Promise
    {
        public string SupposedOffer => "So if you can come, " +
                                       "Caro will inherit everything and you will make our lives happier and see " +
                                       "Blanche is provided for to the end.";

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();

            return (offeror is RupertAndBlancheWhitehead) && (offeree is FrankAndCaroDavis);
        }
    }

    public class AcceptanceAccording2TrialCourt : Performance
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            AddReasonEntry("The offer for specific performance ended with Ruperts death");
            return false;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class AcceptanceApr14 : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is RupertAndBlancheWhitehead) && (offeree is FrankAndCaroDavis);
        }
    }

    public class FrankAndCaroDavis : LegalPerson, IOfferee
    {

    }

    public abstract class RupertAndBlancheWhitehead : LegalPerson
    {
        protected internal RupertAndBlancheWhitehead(string name) : base(name ){ }
    }

    public class RupertWhitehead : RupertAndBlancheWhitehead, IOfferor
    {
        
        public RupertWhitehead() : base("Rupert Whitehead") { }
    }

    public class BlancheWhitehead : RupertAndBlancheWhitehead
    {
        public BlancheWhitehead() : base("Blanche Whitehead") { }
    }
}
