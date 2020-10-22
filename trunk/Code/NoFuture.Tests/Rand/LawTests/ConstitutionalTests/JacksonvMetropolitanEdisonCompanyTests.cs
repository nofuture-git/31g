using System;
using System.Linq;
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

            Func<ILegalPerson[], ILegalPerson> chargedWithDeprivation =
                lps => lps.FirstOrDefault(lp => lp is MetropolitanEdisonCompany);


            var testSubject2 = new RunElectricCompany()
            {
                GetPartyChargedWithDeprivation = chargedWithDeprivation,
                IsSourceStateAuthority = lp => lp is MetropolitanEdisonCompany,
                FairlyDescribedAsStateActor = new StateAction2.TestIsStateActor(chargedWithDeprivation)
                {
                    //not a traditional operation of the government
                    IsTraditionalGovernmentFunction = a => !(a is RunElectricCompany)
                }
            };

            var testResult2 = testSubject2.IsValid(new Jackson(), new MetropolitanEdisonCompany());
            Console.WriteLine(testResult2.ToString());
            Assert.IsFalse(testResult2);
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

    public class RunElectricCompany : StateAction2 { }
}
