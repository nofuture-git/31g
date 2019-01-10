using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts.Ucc;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.UccTests
{
    /// <summary>
    /// FOLEY v. DAYTON BANK & TRUST  Court of Appeals of Tennessee 696 S.W.2d 356 (Tenn.Ct.App. 1985).
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class FoleyvDaytonBankTests
    {
        [Test]
        public void FoleyvDaytonBank()
        {

        }
    }

    public class PurchaseOfTruck : Agreement<Goods>
    {
        public override Predicate<ILegalPerson> IsApprovalExpressed { get; set; } = lp => true;
    }

    public class InternationalTranstarTruck : Goods
    {
        public int Year => 1977;
    }

    public class SellTruck4Cash : InternationalTranstarTruck { }

    public class Foley : VocaBase, ILegalPerson
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
                
                return false;
            }
            return true;
        }
    }
}
