using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.OffersTests
{
    /// <summary>
    /// FAIRMOUNT GLASS WORKS v. CRUNDEN-MARTIN WOODEN WARE CO. Court of Appeals of Kentucky 106 Ky. 659, 51 S.W. 196 (1899)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// The doctrinal point of this case seems to be that the sequence (as in ordered and unique set of telegrams) produced 
    /// an offer and acceptance and therefore is a contract.  The fact that Fairmount sent a, "all sold out" afterwords is 
    /// not good enough.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class FairmountvCrundenMartinTests
    {
        [Test]
        public void FairmountvCrundenMartin()
        {
            var buyer = new CrundenMartin();
            var seller = new Fairmount();
            var getOrigOffer = buyer.GetCommunication();

            var testSubject = new ComLawContract<Promise>
            {
                Offer = new TelegramQuote2CurdenMartin(),
                Acceptance = initRequest => initRequest is TelegramQuote2CurdenMartin ? new TelegramAcceptance2Fairmount() : null,
                Assent = new MutualAssent
                {
                    TermsOfAgreement = lp =>
                    {
                        var isParty = lp is Fairmount
                                      || lp is FairmountCustomer;
                        if (!isParty)
                            return null;

                        switch (lp)
                        {
                            case Fairmount fairmount:
                                return fairmount.GetTerms();
                            case FairmountCustomer cust:
                                return cust.GetTerms();
                        }
                        return null;
                    },

                    IsApprovalExpressed = lp =>
                    {
                        var fairmountAgrees = (lp as Fairmount)?.GetCommunication(getOrigOffer) != null;
                        var cMartinAgrees = (lp as CrundenMartin)?.GetCommunication(seller.GetCommunication(getOrigOffer)) != null;
                        return fairmountAgrees || cMartinAgrees;
                    }
                }
            };
            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {

                IsSoughtByOfferor = (lp, promise) => lp is CrundenMartin && promise is TelegramAcceptance2Fairmount,
                IsGivenByOfferee = (lp, promise) => lp is Fairmount && promise is TelegramQuote2CurdenMartin ,
            };

            var testResult = testSubject.IsValid(new CrundenMartin(), new Fairmount());
            Assert.IsTrue(testResult);
            System.Console.WriteLine(testSubject.ToString());
        }
    }

    public class InitTelegram : LegalConcept
    {
        [Note("Nothing illegal about buying some glass jars")]
        public override bool IsEnforceableInCourt => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is CrundenMartin && offeree is Fairmount
                   || offeror is Fairmount && offeree is CrundenMartin;
        }
    }

    public class TelegramQuote2CurdenMartin : Promise
    {
        public override bool IsEnforceableInCourt => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is CrundenMartin && offeree is Fairmount
                   || offeror is Fairmount && offeree is CrundenMartin;
        }

        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("Pints", 4.5m),
                new Term<object>("Quarts", 5.0m),
                new Term<object>("Half Gallon", 6.5m)
            };
        }
    }

    public class TelegramAcceptance2Fairmount : Promise
    {
        public override bool IsEnforceableInCourt => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is CrundenMartin && offeree is Fairmount
                   || offeror is Fairmount && offeree is CrundenMartin;
        }
    }

    public class TelegramAllSoldOut : Promise
    {
        public override bool IsEnforceableInCourt => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            AddReasonEntry("All sold out");
            return false;
        }
    }

    public class Fairmount : LegalPerson, IOfferee
    {
        public ISet<Term<object>> GetTerms()
        {
            return TelegramQuote2CurdenMartin.GetTerms();
        }

        public virtual LegalConcept GetCommunication(LegalConcept inResponseTo = null)
        {
            if (inResponseTo == null)
                return null;

            if (inResponseTo is InitTelegram)
                return new TelegramQuote2CurdenMartin();
            if(inResponseTo is TelegramAcceptance2Fairmount)
                return new TelegramAllSoldOut();
            return null;
        }
    }

    public abstract class FairmountCustomer : LegalPerson, IOfferor
    {
        public abstract ISet<Term<object>> GetTerms();

        public abstract LegalConcept GetCommunication(LegalConcept inResponseTo = null);
    }

    public class CrundenMartin : FairmountCustomer
    {
        public override LegalConcept GetCommunication(LegalConcept inResponseTo = null)
        {
            if (inResponseTo == null)
                return new InitTelegram();

            if (inResponseTo is TelegramQuote2CurdenMartin)
                return new TelegramAcceptance2Fairmount();

            return null;
        }

        public override ISet<Term<object>> GetTerms()
        {
            return TelegramQuote2CurdenMartin.GetTerms();
        }
    }
}
