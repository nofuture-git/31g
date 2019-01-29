using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.BreachTests
{
    /// <summary>
    /// SMITH v. BRADY Court of Appeals of New York 17 N.Y. 173 (1858)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class SmithvBradyTests
    {
        [Test]
        public void SmithvBrady()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferBuildSomeCottages(),
                Acceptance = o => o is OfferBuildSomeCottages ? new AcceptanceBuildSomeCottages() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Smith _:
                                return ((Smith)lp).GetTerms();
                            case Brady _:
                                return ((Brady)lp).GetTerms();
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
        }
    }

    public class OfferBuildSomeCottages : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Smith || offeror is Brady)
                   && (offeree is Smith || offeree is Brady);
        }
    }

    public class AcceptanceBuildSomeCottages : OfferBuildSomeCottages { }

    public class Smith : LegalPerson
    {
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("build cottages", DBNull.Value),
            };
        }
    }

    public class Brady : LegalPerson
    {
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("build cottages", DBNull.Value),
            };
        }
    }
}
