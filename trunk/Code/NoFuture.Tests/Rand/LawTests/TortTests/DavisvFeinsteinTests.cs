using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.ReasonableCare;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Davis v. Feinstein, 88 A.2d 695 (Pa. 1952)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, afflicted are considered to have met the standard
    /// of care when using compensatory device. Not expected to
    /// actually meet the same nimble results of the unafflicted - only to try.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class DavisvFeinsteinTests
    {
        [Test]
        public void DavisvFeinstein()
        {
            var test = new OfPhysicalDeficiency(ExtensionMethods.Plaintiff)
            {
                IsAfflictedWith = lp => lp is Davis,
                IsUsingCompensatoryDevice = lp => lp is Davis
            };

            var testResult = test.IsValid(new Davis(), new Feinstein());
            Assert.IsTrue(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class Davis : LegalPerson, IPlaintiff
    {
        public Davis(): base("Davis") { }
    }

    public class Feinstein : LegalPerson, ITortfeasor
    {
        public Feinstein(): base("Feinstein") { }
    }
}
