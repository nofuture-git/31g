using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.EstoppelTests
{

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// right arising from acts, admissions, or conduct which have induced a change 
    /// of position in accordance with the real or apparent intention of the party 
    /// against whom they are alleged.
    /// (1) A makes donative promise X to B
    /// (2) B changes conduct based on X
    /// (3) A retracts X
    /// (4) B is left in a position worse than at (1)
    /// (5) X lacked consideration - thereby unenforceable in court
    /// (6) estoppel is used ignore X lacking consideration
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class RicketsvScothornTests
    {
        [Test]
        public void RicketvScothron()
        {
            var testSubject = new LegalContract<DonativePromise>
            {
                Offer = new OfferTwoThousandToGranddaughter(),
            };
            testSubject.Consideration = new PromissoryEstoppel(testSubject);

            var testResult = testSubject.IsValid(new Rickets(), new Scothorn());
            Console.WriteLine(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferTwoThousandToGranddaughter : DonativePromise
    {

    }

    public class Rickets : VocaBase, ILegalPerson
    {
        public Rickets() : base("Andrew D. Ricketts") { }
    }

    public class Scothorn : VocaBase, ILegalPerson
    {
        public Scothorn() : base("Katie Scothorn") { }
    }
}
