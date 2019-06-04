using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Murphy v. Steeplechase Amusement Co., 250 N.Y. 479 (N.Y. 1929)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, plaintiff clearly understood risk therefore no negligence
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MurphyvSteeplechaseAmusementCoTests
    {
        [Test]
        public void MurphyvSteeplechaseAmusementCo()
        {
            var test = new Negligence(ExtensionMethods.Tortfeasor)
            {
                IsNoDuty = lp => lp is SteeplechaseAmusementCo
            };
            var testResult = test.IsValid(new Murphy(), new SteeplechaseAmusementCo());
            Assert.IsFalse(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class Murphy : LegalPerson, IPlaintiff
    {
        public Murphy(): base("Murphy") { }
    }

    public class SteeplechaseAmusementCo : LegalPerson, ITortfeasor
    {
        public SteeplechaseAmusementCo(): base("Steeplechase Amusement Co.") { }
    }
}
