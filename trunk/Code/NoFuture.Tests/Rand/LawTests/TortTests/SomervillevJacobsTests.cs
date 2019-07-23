using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Somerville v. Jacobs, 170 S.E.2d 805 (W. Va. 1969)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, when both parties have acted in good faith court of equity will just make one pay the other to bring balance
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class SomervillevJacobsTests
    {
        [Test]
        public void SomervillevJacobs()
        {
            var test = new ImprovingTrespassers
            {
                SubjectProperty = new LegalProperty("Lot 47") {IsEntitledTo = lp => lp is Somerville},
                Consent = Consent.NotGiven(),
                IsOwnerGoodFaithOblivious = lp => lp is Somerville,
                IsTrespasserGoodFaithIntent = lp => lp is Jacobs
            };

            var testResult = test.IsValid(new Somerville(), new Jacobs());
            Assert.IsTrue(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class Somerville : LegalPerson, IPlaintiff
    {
        public Somerville(): base("Somerville") { }
    }

    public class Jacobs : LegalPerson, ITortfeasor
    {
        public Jacobs(): base("Jacobs") { }
    }
}
