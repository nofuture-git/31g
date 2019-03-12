using System;
using NoFuture.Rand.Law.Contract.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.EstoppelTests
{

    /// <summary>
    /// RICKETTS v. SCOTHORN Supreme Court of Nebraska 57 Neb. 51, 77 N.W. 365 (1898)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// [ right arising from acts, admissions, or conduct which have induced a change 
    /// of position in accordance with the real or apparent intention of the party 
    /// against whom they are alleged.]
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
            var testSubject = new ComLawContract<DonativePromise>
            {
                Offer = new OfferTwoThousandToGranddaughter(),
            };
            testSubject.Consideration = new PromissoryEstoppel<DonativePromise>(testSubject);

            var testResult = testSubject.IsValid(new Rickets(), new Scothorn());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferTwoThousandToGranddaughter : DonativePromise
    {

    }

    public class Rickets : LegalPerson, IOfferor
    {
        public Rickets() : base("Andrew D. Ricketts") { }
    }

    public class Scothorn : LegalPerson, IOfferee
    {
        public Scothorn() : base("Katie Scothorn") { }
    }
}
