using System;
using NoFuture.Rand.Law.Constitutional.US;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Constitutional.Tests
{
    /// <summary>
    /// Jackson v. Metropolitan Edison Co., 419 U.S. 345 (1974)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// there is sufficiently close nexus between theState and the challenged
    /// action of the regulated entity so that the action of the latter may
    /// be fairly treated as that of the State itself
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class JacksonvMetropolitanEdisonCompanyTests
    {
        [Test]
        public void JacksonvMetropolitanEdisonCompany()
        {
            var testSubject = new StateAction
            {
                Consent = Consent.NotGiven(),
                GetActByPerson = p => p is Jackson ? new DisconnectElectricity() : null,
                IsInvidiousDiscrimination = a => false,
                IsPublicCommunity = p => false,
                IsProtectedRight = a => false,
                SubjectProperty = new HomeElectricAccount(),
                IsCloseConnectionToState = a => !(a is DisconnectElectricity)
            };

            var testResult = testSubject.IsValid(new Jackson(), new MetropolitanEdisonCompany());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Jackson : LegalPerson, IPlaintiff
    {
        public Jackson(): base("Jackson") { }
    }

    public class MetropolitanEdisonCompany : LegalPerson, IDefendant
    {
        public MetropolitanEdisonCompany(): base("Metropolitan Edison Company") { }
    }

    public class HomeElectricAccount : RealProperty
    {

    }

    public class DisconnectElectricity : Act
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
