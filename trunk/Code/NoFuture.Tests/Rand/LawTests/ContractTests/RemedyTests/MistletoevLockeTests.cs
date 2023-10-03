using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Law.Contract.US;
using NoFuture.Law.Contract.US.Remedy.MoneyDmg;
using NoFuture.Law.Contract.US.Terms;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Contract.Tests.RemedyTests
{
    /// <summary>
    /// MISTLETOE EXPRESS SERVICE OF OKLAHOMA CITY v. LOCKE Court of Appeals of Texas, Sixth District 762 S.W.2d 637 (Tex. App.—Texarkana 1988)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, recover damages based on reliance in terms of capital investments
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MistletoevLockeTests
    {
        [Test]
        public void MistletoevLocke()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferDeliveryService(),
                Acceptance = o => o is OfferDeliveryService ? new AcceptanctDeliveryService() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Mistletoe _:
                                return ((Mistletoe)lp).GetTerms();
                            case Locke _:
                                return ((Locke)lp).GetTerms();
                            default:
                                return null;
                        }
                    }
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByOfferee = (lp, p) => true,
                IsSoughtByOfferor = (lp, p) => true
            };

            var testResult = testContract.IsValid(new Mistletoe(), new Locke());
            Assert.IsTrue(testResult);

            var testSubject = new Reliance<Promise>(testContract)
            {
                CalcPrepExpenditures = lp =>
                {
                    var locke = lp as Locke;
                    if (locke == null)
                        return 0m;

                    return locke.AmountOfTheLoan - locke.AmountRecoveredFromVehicleSales + locke.CostOfDirtWork +
                           locke.LossFromMaterialsOfRamp;
                }
            };

            testResult = testSubject.IsValid(new Mistletoe(), new Locke());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }
    }

    public class OfferDeliveryService : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Mistletoe || offeror is Locke)
                   && (offeree is Mistletoe || offeree is Locke);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferDeliveryService;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctDeliveryService : OfferDeliveryService
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctDeliveryService;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Mistletoe : LegalPerson, IOfferor
    {
        public Mistletoe(): base("MISTLETOE EXPRESS SERVICE OF OKLAHOMA CITY") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("delivery", DBNull.Value),
            };
        }
    }

    public class Locke : LegalPerson, IOfferee
    {
        public Locke(): base("PHYLLIS LOCKE") { }

        public decimal AmountOfTheLoan => 15000m;
        public decimal AmountRecoveredFromVehicleSales => 6000m;
        public decimal CostOfDirtWork => 1000m;
        public decimal LossFromMaterialsOfRamp => 3000m;

        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("delivery", DBNull.Value),
            };
        }
    }
}
