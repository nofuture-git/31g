using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts.Ucc;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.UccTests
{
    /// <summary>
    /// FOLEY v. DAYTON BANK & TRUST  Court of Appeals of Tennessee 696 S.W.2d 356 (Tenn.Ct.App. 1985).
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, merchant in UCC doesn't just mean any commerical enterprise
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class FoleyvDaytonBankTests
    {
        [Test]
        public void FoleyvDaytonBank()
        {
            var testSubject = new DaytonBank();
            var testGoods = new InternationalTranstarTruck();

            var testResult = testSubject.IsSkilledOrKnowledgeableOf(testGoods);
            Assert.IsFalse(testResult);
            Assert.IsTrue(testSubject.GetReasonEntries().Any());
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class InternationalTranstarTruck : Goods
    {
        public int Year => 1977;
    }

    public class Foley : LegalPerson
    {
        public Foley() : base("Marvin A. Foley, William E. Ball, III, and Johanna M. Foley") { }
    }

    public class DaytonBank : Merchant
    {
        public DaytonBank() : base("Dayton Bank and Trust") { }
        public override bool IsSkilledOrKnowledgeableOf(Goods goods)
        {
            if (goods is InternationalTranstarTruck)
            {
                AddReasonEntry("court found that 'are not merchants' - have no knowledge of big trucks.");
            }
            return false;
        }
    }
}
