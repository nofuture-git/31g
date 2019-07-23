using System;
using NoFuture.Rand.Law.Property.US.Acquisition;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    /// <summary>
    /// O’Keeffe v. Snyder, 83 N.J. 478 (1980)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, adverse possession for chattel is strange since its moveable and concealable
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class OKeeffevSnyderTests
    {
        [Test]
        public void OKeeffevSnyder()
        {
            var test = new AdversePossession(ExtensionMethods.Disseisor)
            {
                Consent = Consent.NotGiven(),
                IsExclusivePossession = lp => lp is Snyder,
                IsContinuousPossession = lp => true,
                IsOpenNotoriousPossession = lp => false,
                SubjectProperty = new OKeeffePaintings()
                {
                    IsEntitledTo = lp => lp is OKeeffe,
                    IsInPossessionOf = lp => lp is Snyder
                },
                Inception = new DateTime(1972,6,1)
            };

            var testResult = test.IsValid(new OKeeffe(), new Snyder());
            Console.WriteLine(test.ToString());
            Assert.IsFalse(testResult);
            
        }
    }

    public class OKeeffePaintings : LegalProperty
    {
        public OKeeffePaintings() : base("three small pictures painted by O’Keeffe") { }
    }

    public class OKeeffe : LegalPerson, IPlaintiff
    {
        public OKeeffe(): base("O’Keeffe") { }
    }

    public class Snyder : LegalPerson, IDefendant, IDisseisor
    {
        public Snyder(): base("Snyder") { }
    }
}
