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
    /// SCOULAR CO. v. DENNEY Court of Appeals of Colorado 151 P.3d 615 (Colo.Ct.App. 2006)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ScoularCovDenneyTests
    {
        [Test]
        public void ScoularCovDenney()
        {

        }
    }

    public class BushelsOfMillet : Goods
    {
        public int Count => 15000;
        public Tuple<decimal,int> PriceKilogram => new Tuple<decimal, int>(5m,50);

    }

    public class FowardContractForMillet : Agreement
    {
        public override Predicate<ILegalPerson> IsApprovalExpressed { get; set; } = lp =>
        {
            if (lp is ScoularCo)
                return true;
            return false;
        };

    }

    public class ScoularCo : VocaBase, ILegalPerson
    {
        public bool IsGrainCompany => true;
    }

    public class Denney : VocaBase, ILegalPerson
    {
        public bool IsGrowerOfMillet => true;
    }
}
