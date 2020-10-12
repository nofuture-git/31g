using System;
using NoFuture.Rand.Law.Constitutional.US;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra;
using NoFuture.Rand.Law.US;
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
            var testSubject = new StateAction()
            {
                Consent = Consent.NotGiven(),
                GetActByPerson = p =>
                {
                    if (p is Kraemer)
                    {
                        return new OwnParcelOfLand()
                        {
                            IsAction = lp => true,
                            IsVoluntary = lp => true
                        };
                    }

                    return null;
                },
                SubjectProperty = new SomeParcelsOfLandForRacist(),
                IsProtectedRight = a => a is OwnParcelOfLand,
                IsPublicCommunity = p => false,
                IsInvidiousDiscrimination = a => a is OwnParcelOfLand
            };

            var testResult = testSubject.IsValid(new Shelley(), new Kraemer());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
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

    public class SomeParcelsOfLandForRacist : RealProperty
    {

    }

    public class OwnParcelOfLand : Act
    {

    }
}
