using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests.DefenseOfOtherTests
{
    /// <summary>
    /// COMMONWEALTH v. Maria A. MIRANDA. No. 08-P-2094. Decided: June 21, 2010
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class CommonwealthvMirandaTests
    {
        [Test]
        public void CommonwealthvMiranda()
        {

        }
    }

    public class Miranda : LegalPerson
    {
        public Miranda() : base("MARIA A. MIRANDA") {}
    }

    public class DemetriaBattle : LegalPerson { }

}
