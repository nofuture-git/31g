using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Courvoisier v. Raymond, 47 P. 284 (Colo. 1896)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class CourvoisiervRaymondTests
    {
        [Test]
        public void CourvoisiervRaymond()
        {

        }
    }

    public class Courvoisier : LegalPerson, IPlaintiff
    {
        public Courvoisier(): base("") { }
    }

    public class Raymond : LegalPerson, ITortfeasor
    {
        public Raymond(): base("") { }
    }
}
