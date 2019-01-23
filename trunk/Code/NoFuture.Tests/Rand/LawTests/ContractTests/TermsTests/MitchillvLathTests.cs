using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.TermsTests
{
    /// <summary>
    /// MITCHILL v. LATH Court of Appeals of New York 247 N.Y. 377, 160 N.E. 646 (1928)
    /// </summary>
    [TestFixture]
    public class MitchillvLathTests
    {
        [Test]
        public void MitchillvLath()
        {

        }

        public ISet<Term<object>> GetTerms()
        {
            var terms = new HashSet<Term<object>>
            {
                new ContractTerm<object>("purchase farm", 1) {Source = TermSource.Written},
                new ContractTerm<object>("remove ice house", 2) {Source = TermSource.Oral}
            };
            return terms;
        }
    }

    public class OfferPurchaseFarmRemIceHouse : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Mitchill || offeror is Lath)
                   && (offeree is Mitchill || offeree is Lath);
        }
    }

    public class AcceptancePurchaseFarmRemIceHouse : OfferPurchaseFarmRemIceHouse { }

    public class Mitchill : LegalPerson
    {

    }

    public class Lath : LegalPerson
    {

    }
}
