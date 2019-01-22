using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.AcceptanceTests
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
                IsSoughtByPromisor = (lp, p) => lp is RupertAndBlancheWhitehead && p is AcceptanceApr14,
                IsGivenByPromisee = (lp, p) => lp is FrankAndCaroDavis && p is OfferApr12
            };

            var testResult = testSubject.IsValid(new RupertWhitehead(), new FrankAndCaroDavis());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());

            var testSubjectAsPerformance = new ComLawContract<Performance>
            {
                Offer = new OfferApr12(),
                Acceptance = o => o is OfferApr12 ? new AcceptanceAccording2TrialCourt() : null
            };
            testSubjectAsPerformance.Consideration = new Consideration<Performance>(testSubjectAsPerformance)
            {
                IsSoughtByPromisor = (lp, p) => lp is RupertAndBlancheWhitehead && p is AcceptanceAccording2TrialCourt,
                IsGivenByPromisee = (lp, p) => lp is FrankAndCaroDavis && p is OfferApr12
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

        public override DateTime? Date { get; set; } = new DateTime(1931, 4, 12);

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is RupertAndBlancheWhitehead) && (offeree is FrankAndCaroDavis);
        }
    }

    public class AcceptanceAccording2TrialCourt : Performance
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            AddReasonEntry("The offer for specific performance ended with Ruperts death");
            return false;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class AcceptanceApr14 : Promise
    {
        public override DateTime? Date { get; set; } = new DateTime(1931, 4, 14);
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is RupertAndBlancheWhitehead) && (offeree is FrankAndCaroDavis);
        }
    }

    public class FrankAndCaroDavis : VocaBase, ILegalPerson
    {

    }

    public abstract class RupertAndBlancheWhitehead : VocaBase, ILegalPerson
    {
        protected internal RupertAndBlancheWhitehead(string name) : base(name ){ }
    }

    public class RupertWhitehead : RupertAndBlancheWhitehead
    {
        
        public RupertWhitehead() : base("Rupert Whitehead") { }
    }

    public class BlancheWhitehead : RupertAndBlancheWhitehead
    {
        public BlancheWhitehead() : base("Blanche Whitehead") { }
    }
}
