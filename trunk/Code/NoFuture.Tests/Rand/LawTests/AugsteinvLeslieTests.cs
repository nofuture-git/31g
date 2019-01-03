using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests
{
    /// <summary>
    /// AUGSTEIN v. LESLIE United States District Court for the Southern District of New York 11 Civ. 7512 (HB), 2012 U.S.Dist.LEXIS 149517 (S.D.N.Y.Oct. 17, 2012)
    /// </summary>
    [TestFixture]
    public class AugsteinvLeslieTests
    {
        [Test]
        public void AugsteinvLeslie()
        {
            var testSubject = new UnilateralContract
            {
                Offer = new TwentyThousandUsdReward(),
                Acceptance = offer =>
                    offer is TwentyThousandUsdReward ? new RyanLesliesLaptopComputerAndExternalHd() : null,

            };
            testSubject.Consideration = new Consideration<Performance>(testSubject)
            {
                IsSoughtByPromisor = (ryan, equipment) =>
                    ryan is RyanLeslie && equipment is RyanLesliesLaptopComputerAndExternalHd,
                IsGivenByPromisee = (augstein, reward) =>
                    augstein is ArminAugstein && reward is TwentyThousandUsdReward
            };

            Assert.IsTrue(testSubject.IsValid(new RyanLeslie(), new ArminAugstein()));

        }

        /// <summary>
        /// This is what Ryan Leslie was offering 
        /// </summary>
        public class TwentyThousandUsdReward : Promise
        {
            private readonly List<string> _audit = new List<string>();
            public override List<string> Audit => _audit;

            public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
            {
                //it has to be ryan leslie
                if (!(promisor is RyanLeslie _))
                {
                    _audit.Add($"{promisor?.Name} is not Ryan Leslie");
                    return false;
                }

                return true;
            }

            /// <summary>
            /// so this would be false if instead of 20,000 USD 
            /// reward, Ryan offered 20 kilos of cocaine.
            /// </summary>
            public override bool IsEnforceableInCourt => true;
        }

        /// <summary>
        /// This is what Ryan Leslie wants back
        /// </summary>
        public class RyanLesliesLaptopComputerAndExternalHd : Performance
        {
            private readonly List<string> _audit = new List<string>();
            public override List<string> Audit => _audit;

            /// <summary>
            /// the court did not consider this the substance of the performance
            /// </summary>
            public bool IsIntellectualPropertyPresent { get; set; }

            public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
            {
                //this either is or isn't the equipment
                return true;
            }

            /// <summary>
            /// This is legal property, if Ryan was trying to 
            /// recover some illegal substance then this would false
            /// </summary>
            public override bool IsEnforceableInCourt => true;
        }

        public class ArminAugstein : VocaBase, ILegalPerson
        {
            public ArminAugstein() : base("Armin Augstein"){ }
        }

        public class RyanLeslie : VocaBase, ILegalPerson
        {
            public RyanLeslie() : base("Ryan Leslie") { }
        }
    }
}
