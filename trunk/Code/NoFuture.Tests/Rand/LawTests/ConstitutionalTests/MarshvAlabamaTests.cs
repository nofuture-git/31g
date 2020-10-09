using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Constitutional.Tests
{
    /// <summary>
    /// Marsh v. Alabama 326 U.S. 501 (1946)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MarshvAlabamaTests
    {
        [Test]
        public void MarshvAlabama()
        {

        }
    }

    public class Marsh : LegalPerson, IPlaintiff
    {
        public Marsh(): base("Marsh") { }
    }

    public class Alabama : LegalPerson, IDefendant
    {
        public Alabama(): base("Alabama") { }
    }
}
