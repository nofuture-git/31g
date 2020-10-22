using System;
using System.Linq;
using NoFuture.Rand.Law.Constitutional.US;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Constitutional.Tests
{
    /// <summary>
    /// Shelley v. Kraemer 334 U.S. 1 (1948)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Attempts to have the full coercive power of the state enforce
    /// private agreements brings the constitutional protections into scope
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ShelleyvKraemerTests
    {
        [Test]
        public void ShelleyvKraemer()
        {
            Func<ILegalPerson[], ILegalPerson> chargedWithDeprivation =
                lps => lps.FirstOrDefault(lp => lp is Kraemer);

            var testSubject2 = new OwnParcelOfLand()
            {
                GetPartyChargedWithDeprivation = chargedWithDeprivation,
                IsSourceStateAuthority = lp => true,
                FairlyDescribedAsStateActor = new StateAction2.TestIsStateActor(chargedWithDeprivation)
                {
                    IsInvidiousDiscrimination = a => a is OwnParcelOfLand
                }
            };

            var testResult2 = testSubject2.IsValid(new Shelley(), new Kraemer());
            Console.WriteLine(testSubject2.ToString());
            Assert.IsTrue(testResult2);
        }
    }

    public class Shelley : LegalPerson, IPlaintiff
    {
        public Shelley(): base("Shelley") { }
    }

    public class Kraemer : LegalPerson, IDefendant
    {
        public Kraemer(): base("Kraemer") { }
    }
    public class OwnParcelOfLand : StateAction2
    {

    }
}
