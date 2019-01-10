using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.UccTests
{
    /// <summary>
    /// COMMERCE & INDUSTRY INS. CO. v. BAYER CORP. Supreme Judicial Court of Massachusetts 433 Mass. 388, 742 N.E.2d 567 (2001)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, detailed break-down of UCC 2-207 contract formation 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class CommerceInsvBayerCorpTests
    {
        [Test]
        public void CommerceInsvBayerCorp()
        {

        }
    }

    public class CommerceIns : LegalPerson
    {
        public CommerceIns() : base("COMMERCE & INDUSTRY INS. CO.") { }
    }

    public class BayerCorp : LegalPerson
    {
        public BayerCorp() : base("BAYER CORP.") { }
    }
}
