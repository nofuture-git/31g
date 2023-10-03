using System;
using System.Linq;
using NoFuture.Law.Constitutional.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Constitutional.Tests
{
    /// <summary>
    /// Edmonson v. Leesville Concrete Company, Inc. 500 U.S. 614 (1991)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Describes the formal predicate logic of State Action in a more abstract way, separate from property
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class EdmonsonvLeesvilleConcreteCompanyIncTests
    {
        [Test]
        public void EdmonsonvLeesvilleConcreteCompanyInc()
        {
            Func<ILegalPerson[], ILegalPerson> chargedWithDeprivation =
                lps => lps.FirstOrDefault(lp => lp is LeesvilleConcreteCompanyInc);
            var testSubject = new PeremptoryChallenge()
            {
                GetPartyChargedWithDeprivation = chargedWithDeprivation,
                //There can be no question that the first part of the Lugar inquiry is satisfied here.
                IsSourceStateAuthority = lp => true,
                FairlyDescribedAsStateActor = new StateAction2.TestIsStateActor(chargedWithDeprivation)
                {
                    //is a function of the courts 
                    IsReliesOnGovernmentAssistance = lp => lp is LeesvilleConcreteCompanyInc,
                    IsTraditionalGovernmentFunction = a => a is PeremptoryChallenge,
                    IsInvidiousDiscrimination = a => a is PeremptoryChallenge
                }
            };

            var testResult = testSubject.IsValid(new Edmonson(), new LeesvilleConcreteCompanyInc());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }
    }

    public class PeremptoryChallenge : StateAction2
    {

    }

    public class Edmonson : LegalPerson, IPlaintiff
    {
        public Edmonson(): base("Edmonson") { }
    }

    public class LeesvilleConcreteCompanyInc : LegalPerson, IDefendant
    {
        public LeesvilleConcreteCompanyInc(): base("Leesville Concrete Company, Inc.") { }
    }
}
