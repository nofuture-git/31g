using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Semiosis;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.LitigateTests
{
    /// <summary>
    /// FRIGALIMENT IMPORTING CO. v. B.N.S. INTERNATIONAL SALES CORP. United States District Court for the Southern District of New York 190 F.Supp. 116 (S.D.N.Y. 1960)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, when a semantic dilemma happens courts will search for meaning in the order as given in Restatement (Second) of Contracts § 203(b)
    /// expressly in contract, course of preformance, course of dealing and usage of trade
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class FrigalimentvBnsInternationalTests
    {
        [Test]
        public void FigalimentvInternational()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferSellChicken(),
                Acceptance = o => o is OfferSellChicken ? new AcceptanceSellChicken() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Frigaliment _:
                                return ((Frigaliment)lp).GetTerms();
                            case BnsInternational _:
                                return ((BnsInternational)lp).GetTerms();
                            default:
                                return null;
                        }
                    }
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };
            var testResult = testContract.IsValid(new Frigaliment(), new BnsInternational());
            //this is true any kind of chicken is the "Chicken" type
            Console.WriteLine(testContract.ToString());
            Assert.IsTrue(testResult);

            var testSubject = new SemanticDilemma<Promise>(testContract)
            {
                IsIntendedMeaningAtTheTime = t => t.Name == "chicken" && t.RefersTo is StewingChicken,
            };
            testResult = testSubject.IsValid(new Frigaliment(), new BnsInternational());
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OrderChickenItem
    {
        public int TotalWeight { get; set; }
        public Chicken Chicken { get; set; }
        public decimal Price { get; set; }
    }

    public class OfferSellChicken : Promise
    {
        public virtual string[] Description => new[]
        {
            "US Fresh Frozen Chicken", "Grade A", "Government Inspected", "Eviscerated",
            "individually wrapped in cryovac", "acked in secured fiber cartons or wooden boxes", "suitable for export"
        };

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Frigaliment || offeror is BnsInternational)
                   && (offeree is Frigaliment || offeree is BnsInternational);
        }

        public virtual List<OrderChickenItem> Order => new List<OrderChickenItem>
        {
            new OrderChickenItem {Chicken = new Chicken(), Price = 33m, TotalWeight = 75000},
            //1.5-2.0 only come as "Young"
            new OrderChickenItem {Chicken = new BroilerOrFryerChicken(), Price = 36.5m, TotalWeight = 25000},
        };
        public virtual DateTime OrderDate => new DateTime(1957,5,2);
    }

    public class SecondOfferSellChicken : OfferSellChicken
    {
        public override List<OrderChickenItem> Order => new List<OrderChickenItem>
        {
            new OrderChickenItem {Chicken = new Chicken(), Price = 33m, TotalWeight = 50000},
            //1.5-2.0 only come as "Young"
            new OrderChickenItem {Chicken = new BroilerOrFryerChicken(), Price = 37m, TotalWeight = 25000},
        };
    }

    public class AcceptanceSellChicken : OfferSellChicken
    {
        //this is what the buyer thought they were getting
        public override List<OrderChickenItem> Order => new List<OrderChickenItem>
        {
            new OrderChickenItem {Chicken = new BroilerOrFryerChicken(), Price = 33m, TotalWeight = 50000},
            new OrderChickenItem {Chicken = new BroilerOrFryerChicken(), Price = 37m, TotalWeight = 25000},
        };
    }

    public class Frigaliment : LegalPerson
    {
        public Frigaliment(): base("FRIGALIMENT IMPORTING CO.") { }

        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                //need at least one shared term of the contract is invalid
                new ContractTerm<object>("order", DBNull.Value, new WrittenTerm(), new UsageOfTradeTerm()),
                //the Frigaliment contents that "chicken" is used throughtout the 
                // industry to mean "broiler fryer chickens" 
                new ContractTerm<object>("chicken", new BroilerOrFryerChicken(), new WrittenTerm(), new UsageOfTradeTerm()),
                new ContractTerm<object>("fowl", new StewingChicken(), new UsageOfTradeTerm()),

                new ContractTerm<object>("brathuhn",new BroilerOrFryerChicken()),
                new ContractTerm<object>("suppenhuhn",new StewingChicken()),
                new ContractTerm<object>("huhn",new Chicken()),
            };
        }
    }

    public class BnsInternational : LegalPerson
    {
        public BnsInternational() : base("B.N.S. INTERNATIONAL SALES CORP.") { }

        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                //need at least one shared term of the contract is invalid
                new ContractTerm<object>("order", DBNull.Value, new WrittenTerm(), new UsageOfTradeTerm()),
                new ContractTerm<object>("chicken", new StewingChicken(), new WrittenTerm())
            };
        }
    }

    public class ErnestBauer : BnsInternational { }

    public class MrStovicek : Frigaliment { }

    /// <summary>
    /// This is the crux of the confusion; namely, the meaing of weight-to-age-to-cookingUse
    /// <![CDATA[
    /// 1.5-2.0     __> 2.5-3.0
    ///   ^   _____/     ^
    ///   |  /           |
    /// Young <_        Old
    ///   ^     \_____   ^
    ///   |           \  |
    /// Broiler         Stew
    /// Fryer
    /// ]]>
    /// </summary>
    public class Chicken
    {
        public override bool Equals(object obj)
        {
            return obj is Chicken;
        }

        public override int GetHashCode()
        {
            return 2;
        }
    }

    public interface IBetween15And20Weight{ }
    public interface IBetween25And30Weight{ }

    public interface IYoungChicken : IBetween15And20Weight, IBetween25And30Weight { }
    public interface IOldChicken : IBetween25And30Weight { }

    public class BroilerOrFryerChicken : Chicken, IYoungChicken
    {
        public override bool Equals(object obj)
        {
            return obj is BroilerOrFryerChicken;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + 1;
        }
    }
    [Aka("Fowl")]
    public class StewingChicken : Chicken, IYoungChicken, IOldChicken
    {
        public override bool Equals(object obj)
        {
            return obj is StewingChicken;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + 1;
        }
    }
}
