using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.IntentionalTort;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Leo v. General Electric Co., 538 N.Y.S.2d 844 (N.Y. App. Div. 1989)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, demo's the idea of private peculiar injury since it was a loss suffered only by commercial fishermen on public waters
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class LeovGeneralElectricCoTests
    {
        [Test]
        public void LeovGeneralElectricCo()
        {
            Predicate<IPlaintiff> isMoneyInjury = lp => lp is Leo;

            var test = new PublicNuisance(ExtensionMethods.Tortfeasor)
            {
                IsProscribedByGovernment = lp => true,
                IsPrivatePeculiarInjury = lp => lp is Leo && isMoneyInjury(lp)
            };

            var testResult = test.IsValid(new Leo(), new GeneralElectricCo());
            Assert.IsTrue(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class Leo : LegalPerson, IPlaintiff
    {
        public Leo(): base("Leo") { }
    }

    public class GeneralElectricCo : LegalPerson, ITortfeasor
    {
        public GeneralElectricCo(): base("General Electric Co.") { }
    }
}
