using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Constitutional.US;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Constitutional.Tests
{
    /// <summary>
    /// Marsh v. Alabama 326 U.S. 501 (1946)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, property which acts as a public community is protected regardless of
    /// who has title
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MarshvAlabamaTests
    {
        [Test]
        public void MarshvAlabama()
        {
            Func<ILegalPerson[], ILegalPerson> chargedWithDeprivation =
                lps => lps.FirstOrDefault(lp => lp is Alabama);
            var testSubject2 = new OperationOfCompanyOwnedTown
            {
                GetPartyChargedWithDeprivation = chargedWithDeprivation,
                IsSourceStateAuthority = lp => lp is Alabama,
                FairlyDescribedAsStateActor = new StateAction2.TestIsStateActor(chargedWithDeprivation)
                {
                    IsTraditionalGovernmentFunction = a => a is OperationOfCompanyOwnedTown
                }
            };

            var testResult2 = testSubject2.IsValid(new Marsh(), new Alabama());
            Console.WriteLine(testSubject2.ToString());
            Assert.IsTrue(testResult2);
        }
    }

    public class OperationOfCompanyOwnedTown : StateAction2
    {

    }

    public class Marsh : LegalPerson, IDefendant
    {
        public Marsh(): base("Marsh") { }
    }

    public class Alabama : LegalPerson, IPlaintiff
    {
        public Alabama(): base("Alabama") { }
    }
}
