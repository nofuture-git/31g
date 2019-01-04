using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.OffersTests
{
    /// <summary>
    /// LEONARD v. PEPSICO, INC. United States District Court for the Southern District of New York 88 F.Supp. 2d 116 (S.D.N.Y. 1999)
    /// </summary>
    [TestFixture]
    public class LeonardvPepsicoIncTests
    {
        [Test]
        public void LeonardvPepsicoInc()
        {
            var testSubject = new BilateralContract
            {
                MutualAssent = new PepsiStuffCatalogOrderForm(),
                Offer = new PepsiStuffCommercial(),
                Acceptance = orderForm => orderForm is PepsiPoints ? new PepsiStuff() : null,
            };
            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {
                IsSoughtByPromisor = (lp, promise) => lp is PepsicoInc && promise is PepsiStuff,
                IsGivenByPromisee = (lp, promise) => lp is ConsumerOfPepsi && promise is PepsiPoints
            };

            var testResult = testSubject.IsValid(new PepsicoInc(), new Leonard());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }


    /// <summary>
    /// This is the actual offer
    /// </summary>
    public class PepsiStuffCatalogOrderForm : MutualAssent
    {
        public PepsiStuffCatalogOrderForm()
        {
            //does everybody understand what "pepsi stuff" and "pepsi points" are
            TermsOfAgreement = lp =>
            {
                var isParty = lp is PepsicoInc
                              || lp is ConsumerOfPepsi;
                if (!isParty)
                    return null;

                switch (lp)
                {
                    case PepsicoInc pepsicoInc:
                        return pepsicoInc.GetTerms();
                    case ConsumerOfPepsi consumerOfPepsi:
                        return consumerOfPepsi.GetTerms();
                }

                return null;
            };

            IsApprovalExpressed = lp =>
            {
                switch (lp)
                {
                    case ConsumerOfPepsi consumerOfPepsi:
                        return consumerOfPepsi.PepsiPoints?.Value > 0;
                    case PepsicoInc _:
                        return true;
                }

                return false;
            };
        }

        private Dictionary<string, int> _catalog = new Dictionary<string, int>
        {
            {"Blue Shades", 175},
            {"Leather Jacket", 1450},
            {"Pepsi Tees", 75},
            {"Bag of Balls", -1},
            {"Pepsi Phone Card", -1},
            {"Jacket Tattoo", 15},
            {"Fila Mountain Bike", 3300}
        };

        /// <summary>
        /// There does not appear any disagreement about what 
        /// pepsi stuff and pepsi points are amoung any involved.
        /// </summary>
        /// <returns></returns>
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("Pepsi Points", new PepsiPoints()),
                new Term<object>("Pepsi Stuff", new PepsiStuff())
            };
        }
    }


    /// <summary>
    /// This is what Leonard was using as an offer
    /// </summary>
    public class PepsiStuffCommercial : Advertisement
    {

    }


    /// <summary>
    /// This is what Pepsi wants from its consumers
    /// </summary>
    public class PepsiPoints : PepsiStuff
    {
        public int Value { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PepsiPoints;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + 1;
        }
    }

    /// <summary>
    /// This is what Pepsi is promising in return for points
    /// </summary>
    public class PepsiStuff : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is PepsicoInc && offeree is ConsumerOfPepsi;
        }

        public override bool IsEnforceableInCourt => true;

        public override bool Equals(object obj)
        {
            return obj is PepsiStuff;
        }

        public override int GetHashCode()
        {
            return 25154;
        }
    }

    public abstract class ConsumerOfPepsi : VocaBase, ILegalPerson
    {
        protected internal ConsumerOfPepsi(string name) : base(name) { }

        public PepsiPoints PepsiPoints { get; set; }

        public ISet<Term<object>> GetTerms()
        {
            return PepsiStuffCatalogOrderForm.GetTerms();
        }
    }

    public class Leonard : ConsumerOfPepsi
    {
        public Leonard() : base("LEONARD")
        {
            PepsiPoints = new PepsiPoints {Value = 7000000};
        }
    }

    public class PepsicoInc : VocaBase, ILegalPerson
    {
        public PepsicoInc() : base("PEPSICO, INC.") { }

        public ISet<Term<object>> GetTerms()
        {
            return PepsiStuffCatalogOrderForm.GetTerms();
        }
    }
}
