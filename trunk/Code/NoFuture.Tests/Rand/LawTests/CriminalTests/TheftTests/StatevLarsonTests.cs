using System;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.US.Property;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.TheftTests
{
    /// <summary>
    /// State v. Larson, 605 N.W. 2d 706 (2000)
    /// </summary>
    /// <remarks>
    ///<![CDATA[
    /// doctrine issue, security deposit is a debt and a person cannot be put in jail for not paying a debt.
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class StatevLarsonTests
    {
        [Test]
        public void StatevLarson()
        {
            var securityDeposit = new SecurityDeposit();
            var larson = new Larson();
            var lessee = new TheLesseeEg();
            var kindOrRelationship = Larson.GetKindOfRelationship();
            switch (kindOrRelationship)
            {
                //the deposit is a debt 
                case "debtor-creditor":
                    securityDeposit.IsAllowedToCommingle = true;
                    securityDeposit.EntitledTo = larson;
                    break;
                case "pledgor-pledgee":
                    securityDeposit.IsAllowedToCommingle = true;
                    securityDeposit.EntitledTo = lessee;
                    break;
                case "settlor-trustee":
                    securityDeposit.IsAllowedToCommingle = false;
                    securityDeposit.EntitledTo = lessee;
                    break;
            }

            var testCrime = new Felony
            {
                ActusReus = new ByTaking
                {
                    SubjectProperty = securityDeposit,
                    Consent = new VictimConsent
                    {
                        IsCapableThereof = lp => true,
                        IsApprovalExpressed = lp => true,
                    },
                    IsTakenPossession = lp => lp is Larson,
                    IsAsportation = lp => lp is Larson
                },
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is Larson
                }
            };

            var testResult = testCrime.IsValid(larson, lessee);
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);

        }
    }

    public class TheLesseeEg : LegalPerson
    {
        public TheLesseeEg() : base("LESSEE") {  }
    }

    public class SecurityDeposit : PersonalProperty
    {
        public bool IsAllowedToCommingle { get; set; }
    }

    public class Larson : LegalPerson, IDefendant
    {
        public Larson() : base("FRANK DONALD LARSON") { }

        public static  string GetKindOfRelationship()
        {
            return "debtor-creditor";
        }
    }
}
