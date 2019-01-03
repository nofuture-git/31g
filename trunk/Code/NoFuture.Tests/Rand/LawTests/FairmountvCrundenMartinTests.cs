﻿using NUnit.Framework;
using NoFuture.Rand.Law;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Contracts;
using System.Collections.Generic;

namespace NoFuture.Rand.Tests.LawTests
{
    /// <summary>
    /// FAIRMOUNT GLASS WORKS v. CRUNDEN-MARTIN WOODEN WARE CO. Court of Appeals of Kentucky 106 Ky. 659, 51 S.W. 196 (1899)
    /// The doctrinal point of this case seems to be that the sequence (as in ordered and unique set of telegrams) produced 
    /// mutual assent in the parties.
    /// </summary>
    [TestFixture]
    public class FairmountvCrundenMartinTests
    {

        [Test]
        public void FairmountvCrundenMartin()
        {
            var buyer = new CrundenMartin();
            var seller = new Fairmount();
            var getOrigOffer = buyer.GetCommunication();

            var testSubject = new BilateralContract
            {
                Consideration = new BilateralConsideration
                {
                    Offer = getOrigOffer,
                    GetInReturnFor = initRequest => seller.GetCommunication(initRequest) as Promise,
                    IsSoughtByPromisor = (lp, promise) =>
                    {
                        var fairmount = lp as Fairmount;
                        return fairmount.GetCommunication(promise) is Apr23rdResponseTelegram;
                    },
                    IsGivenByPromisee = (lp, promise) =>
                    {
                        var cMartin = lp as CrundenMartin;
                        return cMartin.GetCommunication(promise) is Apr24thResponseTelegram;
                    },
                },

                MutualAssent = new MutualAssent
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

            var testResult = testSubject.IsValid(buyer, seller);
            System.Console.WriteLine(testResult);
        }
    }

    public class Apr20InitialTelegram : ObjectiveLegalConcept
    {
        [Note("Nothing illegal about buying some glass jars")]
        public override bool IsEnforceableInCourt => true;

        public override List<string> Audit => new List<string>();

        public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            return (promisor is CrundenMartin && promisee is Fairmount)
                   || (promisor is Fairmount && promisee is CrundenMartin);
        }
    }

    public class Apr23rdResponseTelegram : Promise
    {
        public override bool IsEnforceableInCourt => true;

        public override List<string> Audit => new List<string>();

        public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            return (promisor is CrundenMartin && promisee is Fairmount)
                   || (promisor is Fairmount && promisee is CrundenMartin);
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

    public class Apr24thResponseTelegram : Promise
    {
        public override bool IsEnforceableInCourt => true;

        public override List<string> Audit => new List<string>();

        public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            return (promisor is CrundenMartin && promisee is Fairmount)
                   || (promisor is Fairmount && promisee is CrundenMartin);
        }
    }

    public class Fairmount : VocaBase, ILegalPerson
    {
        public ISet<Term<object>> GetTerms()
        {
            return Apr23rdResponseTelegram.GetTerms();
        }

        public virtual ObjectiveLegalConcept GetCommunication(ObjectiveLegalConcept inResponseTo = null)
        {
            if (inResponseTo == null)
                return null;

            if (inResponseTo is Apr20InitialTelegram)
                return new Apr23rdResponseTelegram();

            return null;
        }
    }

    public abstract class FairmountCustomer : VocaBase, ILegalPerson
    {
        public abstract ISet<Term<object>> GetTerms();

        public abstract ObjectiveLegalConcept GetCommunication(ObjectiveLegalConcept inResponseTo = null);
    }

    public class CrundenMartin : FairmountCustomer
    {
        public override ObjectiveLegalConcept GetCommunication(ObjectiveLegalConcept inResponseTo = null)
        {
            if (inResponseTo == null)
                return new Apr20InitialTelegram();

            if (inResponseTo is Apr23rdResponseTelegram)
                return new Apr24thResponseTelegram();

            return null;
        }

        public override ISet<Term<object>> GetTerms()
        {
            return Apr23rdResponseTelegram.GetTerms();
        }
    }
}
