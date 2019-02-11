using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.OffersTests
{
    /// <summary>
    /// KLOIAN v. DOMINO’S PIZZA, L.L.C. Court of Appeals of Michigan 273 Mich.App. 449, 733 N.W.2d 766 (2006)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// This seems to be another example that once you have all the requirements of a contract then you have a 
    /// contract - the only way out now is if both parties want out.
    /// ]]>
    /// </remarks>
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

        [Test]
        public void KloianvDominosPizza()
        {
            var testSubject = new ComLawContract<Promise>
            {
                Offer = new OfferDefendantPay2SettleLawsuit(),
                Acceptance = o =>
                    o is OfferDefendantPay2SettleLawsuit ? new AcceptanceEmailFromPlaintiffAttorney() : null,
                Assent = new MutualAssent
                {
                    TermsOfAgreement = lp =>
                    {
                        var isParty = lp is PartiesInCase;
                        return !isParty ? null : GetTerms();
                    },
                    IsApprovalExpressed = lp => (lp as PartiesInCase)?.IsApprovalExpressed ?? false
                },
            };

            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {
                IsSoughtByPromisor = (lp, promise) => lp is EdwardKloian && promise is AcceptanceEmailFromPlaintiffAttorney,
                IsGivenByPromisee = (lp, promise) => lp is DominosPizzaLlc && promise is OfferDefendantPay2SettleLawsuit,
            };

            var testResult = testSubject.IsValid(new EdwardKloianAttorney(), new DominosPizzaLlcAttorney());
            
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        private static object _termMeaning = new object();

        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("payment", 48000m),
                new Term<object>("dismissal with prejudice of all claims and release of all possible claims", _termMeaning)
            };
        }
    }

    public class OfferDefendantPay2SettleLawsuit : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is PartiesInCase && offeree is PartiesInCase;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public abstract class EmailInCase : Promise
    {
        public abstract string EmailText { get; }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is PartiesInCase && offeree is PartiesInCase;
        }

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
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return false;
        }

        public override bool IsEnforceableInCourt => true;

    }

    public abstract class PartiesInCase : VocaBase, ILegalPerson
    {
        protected PartiesInCase(string name) : base(name) { }

        public abstract LegalConcept GetCommunication(LegalConcept inResponseTo = null);
        public abstract bool IsApprovalExpressed { get; }
    }

    public class EdwardKloianAttorney : EdwardKloian
    {
        public EdwardKloianAttorney() :base("Edward Kloian's Attorney") { }

        public override LegalConcept GetCommunication(LegalConcept inResponseTo = null)
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

        public override bool IsApprovalExpressed => true;
    }

    public class DominosPizzaLlcAttorney : DominosPizzaLlc
    {
        public DominosPizzaLlcAttorney(): base("Dominos Pizza LLC Attorney") { }

        public override LegalConcept GetCommunication(LegalConcept inResponseTo = null)
        {
            if(inResponseTo == null)
                return new OfferDefendantPay2SettleLawsuit();
            if(inResponseTo is AcceptanceEmailFromPlaintiffAttorney)
                return new AcceptanceEmailFromDefendantAttorney();
            if(inResponseTo is ReviewedDoxWantMutualRelease)
                return new ResponseAboutMutualRelease();
            return null;
        }
        public override bool IsApprovalExpressed => true;
    }

    public class EdwardKloian : PartiesInCase
    {
        public EdwardKloian(string name) : base (name ?? "Edward Kloian") { }

        public EdwardKloianAttorney Attorney => new EdwardKloianAttorney();
        public override LegalConcept GetCommunication(LegalConcept inResponseTo = null)
        {
            return Attorney.GetCommunication(inResponseTo);
        }
        public override bool IsApprovalExpressed => Attorney.IsApprovalExpressed;
    }

    public class DominosPizzaLlc : PartiesInCase
    {
        public DominosPizzaLlc(string name)  : base(name ?? "Dominos Pizza LLC") { }

        public DominosPizzaLlcAttorney Attorney => new DominosPizzaLlcAttorney();
        public override LegalConcept GetCommunication(LegalConcept inResponseTo = null)
        {
            return Attorney.GetCommunication(inResponseTo);
        }
        public override bool IsApprovalExpressed => Attorney.IsApprovalExpressed;
    }
}
