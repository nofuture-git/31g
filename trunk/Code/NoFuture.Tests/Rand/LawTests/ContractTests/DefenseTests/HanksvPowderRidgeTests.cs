using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Defense.ToPublicPolicy;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.DefenseTests
{
    [TestFixture()]
    public class HanksvPowderRidgeTests
    {
        [Test]
        public void HanksvPowderRidge()
        {
            var testContract = new BilateralContract
            {
                Offer = new OfferSnowTubing(),
                Acceptance = o => o is OfferSnowTubing ? new AcceptSnowTubing() : null,
                MutualAssent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => GetTerms()
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };

            var testSubject = new ByLimitTortLiability<Promise>(testContract);

            var testResult = testSubject.IsValid(new PowderRidge(), new Hanks());
            Console.WriteLine(testSubject.ToString());
        }
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("snow tubing", DBNull.Value)
            };
        }
    }

    public class OfferSnowTubing : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Hanks || offeror is PowderRidge)
                   && (offeree is Hanks || offeree is PowderRidge);
        }

        public int MinAgeRequirement => 6;
        public Tuple<int, string> MinHeightRequirement = new Tuple<int, string>(44, "inches");

    }

    public class AcceptSnowTubing : OfferSnowTubing { }

    public class Hanks : LegalPerson
    {

    }

    public class PowderRidge : LegalPerson
    {
        public PowderRidge(): base("POWDER RIDGE RESTAURANT CORP.") { }
    }
}
