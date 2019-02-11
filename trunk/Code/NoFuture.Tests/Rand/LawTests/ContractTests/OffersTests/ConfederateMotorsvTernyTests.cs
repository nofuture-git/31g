using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.OffersTests
{
    /// <summary>
    /// CONFEDERATE MOTORS, INC. v. TERNY United States District Court for the District of Massachusetts 831 F.Supp. 2d 414 (D.Mass. 2011)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine on this one is about an offer expiring.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ConfederateMotorsvTernyTests
    {
        [Test]
        public void ConferateMotorsvTerny()
        {
            var testSubject = new ComLawContract<Promise>
            {
                Offer = new OfferDec28th(),
                Acceptance = o => new AcceptanceTooLate(),
                //didn't ever bother with Consideration and MutualAssent since this is DOA
            };

            var testResult = testSubject.IsValid(new ConfedMotorsInc(), new Terny());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        private static object _term00 = new object();

        public static ISet<Term<object>> GetTerms => new HashSet<Term<object>>
        {
            new Term<object>("mutual release of all existing claims", _term00),
            new Term<object>("consulting shares", _term00),
            new Term<object>("dollars", _term00),
        };
    }

    

    /// <summary>
    /// from Confed to Terny
    /// </summary>
    public class OfferDec9th : Promise
    {
        public int ConsultingShared => 505000;
        public decimal Usd => 150000m;

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is ConfedMotorsInc && offeree is Terny;
        }

        public override bool IsEnforceableInCourt => true;
    }

    /// <summary>
    /// from Terny to Confed
    /// </summary>
    public class OfferDec15th : Promise
    {
        public int ConsultingShared => 505000;
        public decimal Usd => 0m;

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is ConfedMotorsInc && offeree is Terny;
        }

        public override bool IsEnforceableInCourt => true;
    }

    /// <summary>
    /// from Confed to Terny
    /// </summary>
    public class OfferDec22nd : Promise
    {
        public int ConsultingShared => 505000;
        public decimal Usd => 100000m;
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is ConfedMotorsInc && offeree is Terny;
        }

        public override bool IsEnforceableInCourt => true;
    }

    /// <summary>
    /// from Terny to Confed
    /// </summary>
    public class OfferDec28th : OfferDec15th
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            AddReasonEntry("court found this was too late, offer had expired.");
            return false;
        }
    }

    public class AcceptanceTooLate : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is ConfedMotorsInc && offeree is Terny;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class AttorneyTurner : ConfedMotorsInc
    {
        public AttorneyTurner() : base("Chance Turner (attorney for Confederate)") { }
    }

    public class AttorneyMcDuff : Terny
    {
        public AttorneyMcDuff() : base("Laurence McDuff (attorney for Terny)") { }
    }

    public class ConfedMotorsInc : LegalPerson
    {
        public ConfedMotorsInc(string name) : base(name) { }
        public ConfedMotorsInc() : this("CONFEDERATE MOTORS, INC.") { }
    }

    public class Terny : LegalPerson
    {
        public Terny(string name) : base(name) { }
        public Terny() : this("Francois-Xavier Terny") { }
    }
}
