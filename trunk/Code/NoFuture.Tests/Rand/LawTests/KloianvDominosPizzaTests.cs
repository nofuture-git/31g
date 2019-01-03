using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NoFuture.Rand.Tests.LawTests
{
    /// <summary>
    /// KLOIAN v. DOMINO’S PIZZA, L.L.C. Court of Appeals of Michigan 273 Mich.App. 449, 733 N.W.2d 766 (2006)
    /// </summary>
    [TestFixture]
    public class KloianvDominosPizzaTests
    {
        [Test]
        public void TestAgreedPaySettlement()
        {
            var ekLawyer = new EdwardKloianAttorney();
            var dpLawyer = new DominosPizzaLlcAttorney();

            var testSubject = new OfferDefendantPay2SettleLawsuit();
            Assert.IsTrue(testSubject.IsValid(ekLawyer, dpLawyer));
        }
       
    }

    public class TestSubject : BilateralContract
    {
        public override ObjectiveLegalConcept Offer { get; set; } = new OfferDefendantPay2SettleLawsuit();

        public override Func<ObjectiveLegalConcept, Promise> Acceptance { get; set; } = o =>
            o is OfferDefendantPay2SettleLawsuit ? new AcceptanceEmailFromPlaintiffAttorney() : null;

        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("payment", 48000m),
                new Term<object>("dismissal with prejudice of all claims and release of all possible claims", new object())
            };
        }
    }

    public class OfferDefendantPay2SettleLawsuit : Promise
    {
        public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            return promisor is PartiesInCase && promisee is PartiesInCase;
        }

        public override List<string> Audit => new List<string>();
        public override bool IsEnforceableInCourt => true;
    }

    public abstract class EmailInCase : Promise
    {
        public abstract string EmailText { get; }

        public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            return promisor is PartiesInCase && promisee is PartiesInCase;
        }

        public override List<string> Audit => new List<string>();
        public override bool IsEnforceableInCourt => true;
    }

    public class AcceptanceEmailFromPlaintiffAttorney : EmailInCase
    {
        public override string EmailText => "I confirmed with Mr. Kloian that he " +
                                   "will accept the payment of $48,000 in " +
                                   "exchange for a dismissal with prejudice " +
                                   "of all claims and a release of all " +
                                   "possible claims.";
    }

    public class AcceptanceEmailFromDefendantAttorney : EmailInCase
    {
        public override string EmailText => "Domino’s accepts your settlement offer.";
    }

    public class ReviewedDoxWantMutualRelease : EmailInCase
    {
        public override string EmailText => "I reviewed your documents and find them " +
                                            "to be in order. However, Mr. Kloian would " +
                                            "like the protection of a mutual release.";
    }

    public class ResponseAboutMutualRelease : EmailInCase
    {
        public override string EmailText => "I have the check and Domino’s agreement to " +
                                            "a mutual release. I need to revise the prior " +
                                            "release and get it to you.";
    }

    public class SubsequentRefusal2Sign : Promise
    {
        public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            return false;
        }

        public override List<string> Audit => new List<string>();
        public override bool IsEnforceableInCourt => true;

    }

    public abstract class PartiesInCase : VocaBase, ILegalPerson
    {
        public abstract ObjectiveLegalConcept GetCommunication(ObjectiveLegalConcept inResponseTo = null);
    }

    public class EdwardKloianAttorney : PartiesInCase
    {
        public override ObjectiveLegalConcept GetCommunication(ObjectiveLegalConcept inResponseTo = null)
        {
            if (inResponseTo == null)
                return null;
            if(inResponseTo is OfferDefendantPay2SettleLawsuit)
                return new AcceptanceEmailFromPlaintiffAttorney();
            if(inResponseTo is AcceptanceEmailFromDefendantAttorney)
                return new ReviewedDoxWantMutualRelease();
            if (inResponseTo is ResponseAboutMutualRelease)
                return new SubsequentRefusal2Sign();

            return null;
        }
    }

    public class DominosPizzaLlcAttorney : PartiesInCase
    {
        public override ObjectiveLegalConcept GetCommunication(ObjectiveLegalConcept inResponseTo = null)
        {
            if(inResponseTo == null)
                return new OfferDefendantPay2SettleLawsuit();
            if(inResponseTo is AcceptanceEmailFromPlaintiffAttorney)
                return new AcceptanceEmailFromDefendantAttorney();
            if(inResponseTo is ReviewedDoxWantMutualRelease)
                return new ResponseAboutMutualRelease();
            return null;
        }
    }

    public class EdwardKloian : PartiesInCase
    {
        public EdwardKloianAttorney Attorney => new EdwardKloianAttorney();
        public override ObjectiveLegalConcept GetCommunication(ObjectiveLegalConcept inResponseTo = null)
        {
            return Attorney.GetCommunication(inResponseTo);
        }
    }

    public class DominosPizzaLlc : PartiesInCase
    {
        public DominosPizzaLlcAttorney Attorney => new DominosPizzaLlcAttorney();
        public override ObjectiveLegalConcept GetCommunication(ObjectiveLegalConcept inResponseTo = null)
        {
            return Attorney.GetCommunication(inResponseTo);
        }
    }
}
