using System;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Ucc;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.UccTests
{
    /// <summary>
    /// COMPUTER NETWORK, LTD. v. PURCELL TIRE & RUBBER COMPANY Missouri Court of Appeals 747 S.W.2d 669 (Mo.App. 1988)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, the concept of Mutual Assent gets applied here for a UCC contract on goods.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ComNetworkvPurcellTireTests
    {
        [Test]
        public void ComNetworkvPurcellTire()
        {
            var testSubject = new UccContract<Goods>
            {
                Assent = new LetterFromCBrown{ IsApprovalExpressed = lp => true},
                Offer = new IbmPersonalComputers(),
                Acceptance = o => new IbmPersonalComputers()
            };
            var testResult = testSubject.IsValid(new ComNetwork(), new PurcellTire());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class IbmPersonalComputers : Goods
    {
        public int GetOrderCount(ILegalPerson lp)
        {
            if (lp is ComNetwork)
                return 21;
            if (lp is PurcellTire)
                return 12;
            return 0;
        }
    }

    public class LetterFromCBrown : Agreement
    {
        /// <summary>
        /// The court finds that both parties manifest assent based on the written
        /// agreement
        /// </summary>
        public override Predicate<ILegalPerson> IsApprovalExpressed { get; set; } =
            lp => lp is ComNetwork || lp is PurcellTire;
    }

    public class ComNetwork : LegalPerson, IOfferor
    {
        public ComNetwork(): base("COMPUTER NETWORK, LTD.") { }
    }

    public class PurcellTire : LegalPerson, IOfferee
    {
        public PurcellTire() : base("PURCELL TIRE & RUBBER COMPANY") { }
    }
}
