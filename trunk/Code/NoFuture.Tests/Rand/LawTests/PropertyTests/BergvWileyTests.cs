using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    /// <summary>
    /// Berg v. Wiley, 264 N.W.2d 145 (Minn. 1978).
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class BergvWileyTests
    {
        [Test]
        public void BergvWiley()
        {
            var test = new Leasehold()
            {
                Inception = new DateTime(1970, 12, 1),
                SubjectProperty = new RealProperty("building in Osseo, Minnesota"),
                Terminus = new DateTime(1975, 12, 1)
            };

            var testResult = test.IsValid(new Berg(), new Wiley());
            Assert.IsTrue(testResult);
        }
    }

    public class Berg : LegalPerson, IPlaintiff, ILessee
    {
        public Berg(): base("Berg") { }
    }

    public class Wiley : LegalPerson, IDefendant, ILessor
    {
        public Wiley(): base("Wiley") { }
    }
}
