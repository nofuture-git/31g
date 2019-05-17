using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Mohr v. Williams, 104 N.W. 12, 13-16 (Minn. 1905)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MohrvWilliamsTests
    {
        [Test]
        public void MohrvWilliams()
        {

        }
    }

    public class Mohr : LegalPerson, IPlaintiff
    {
        public Mohr(): base("") { }
    }

    public class Williams : LegalPerson, ITortfeasor
    {
        public Williams(): base("") { }
    }
}
