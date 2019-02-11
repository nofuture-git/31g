using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.AcceptanceTests
{
    /// <summary>
    /// UNITED STATES LIFE INSURANCE CO. v. WILSON Court of Special Appeals of Maryland 198 Md.App. 452, 18 A.3d 110 (2011)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue here looks to be the "mailbox rule"
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class UnitedStatesLifeInsCovWilsonTests
    {
        [Test]
        public void UnitedStatesLifeInsCovWilson()
        {
            var testSubject = new ComLawContract<Performance>
            {
                Offer = new OfferLifeInsPolicy(),
                Acceptance = o => o is OfferLifeInsPolicy ? new AcceptanceByPayInsPremium() : null,
            };
            testSubject.Consideration = new Consideration<Performance>(testSubject)
            {
                IsSoughtByPromisor = (lp, p) => true,
                IsGivenByPromisee = (lp, p) => true
            };

            var testResult = testSubject.IsValid(new UnitedStatesLifeInsCo(), new Wilson());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferLifeInsPolicy : Promise
    {
        public string LongName => "American Medical Association-Sponsored Group Level Term Life Insurance Policy";
        public string CertNumber => "9500108167";
        public Tuple<string, int> Term => new Tuple<string, int>("years", 10);

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is UnitedStatesLifeInsCo && offeree is Wilson;
        }

        public Dictionary<DateTime, string> DatesOfFacts => new Dictionary<DateTime, string>
        {
            {new DateTime(2007, 5, 14), "BILL NOTICE"},
            {new DateTime(2007, 5, 16), "REMINDER NOTICE"},
            {new DateTime(2007, 7, 23), "directed pay premium on 7/25/07 delivered on 7/30/07"},
            {new DateTime(2007, 7, 28), "Dr. Griffith died"},
            {new DateTime(2007, 7, 30), "AMAIA received payment"},
            {new DateTime(2007, 8, 2), "rejected payment"},
            {new DateTime(2007, 9, 28), "submitted a claim to AMAIA for death benefit"},
            {new DateTime(2008, 4, 14), "AMAIA deny claim"}
        };
    }

    public class AcceptanceByPayInsPremium : Performance
    {
        public DateTime DueDate => new DateTime(2007,5,15);
        /// <summary>
        /// This got extended to 60 days in the Reminder notice
        /// </summary>
        public TimeSpan ExtendedGracePeriod = new TimeSpan(60,0,0,0);

        /// <summary>
        /// Days from end of grace period in which a policy is reinstated 
        /// without needing written approval.
        /// </summary>
        public TimeSpan WrittenApprovalNotRequired = new TimeSpan(31,0,0,0);

        /// <summary>
        /// The date scheduled to have payment sent, Dr. Griffith died two days later
        /// </summary>
        public DateTime DateOfPayment { get; set; } = new DateTime(2007,7,25);

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!(offeror is UnitedStatesLifeInsCo) || !(offeree is Wilson))
            {
                AddReasonEntry("incorrect person given");
                return false;
            }

            //policy is such that you add the grace period to the due date then from 
            //that add the 31 days and up to that you get reinstated without written approval
            var duePlusGrace = DueDate.Add(ExtendedGracePeriod);
            var duePlusGraceAnd31 = duePlusGrace.Add(WrittenApprovalNotRequired);
            var r = duePlusGraceAnd31 > DateOfPayment;
            if (!r)
            {
                AddReasonEntry($"due date plus extended grace period {duePlusGrace.ToShortDateString()}");
                AddReasonEntry($"the additional 31 days before written notice { duePlusGraceAnd31.ToShortDateString()}");
            }

            return r;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class UnitedStatesLifeInsCo : LegalPerson
    {
        public UnitedStatesLifeInsCo() : base("United States Life Insurance Company") { }
    }

    public class Wilson : LegalPerson
    {
        public Wilson() : base("Elizabeth Wilson") { }
    }
}
