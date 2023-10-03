using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Law.Constitutional.US;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Constitutional.Tests
{
    /// <summary>
    /// Manhattan Community Access Corporation v. Halleck 588 U.S. ___ (2019)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// example of when one is clearly not protected by constitutional rights
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ManhattanCommunityAccessCorporationvHalleckTests
    {
        [Test]
        public void ManhattanCommunityAccessCorporationvHalleck()
        {
            Func<ILegalPerson[], ILegalPerson> chargedWithDeprivation =
                lps => lps.FirstOrDefault(lp => lp is ManhattanCommunityAccessCorporation);

            var testSubject = new OperationOfPublicAccessCableChannels
            {
                GetPartyChargedWithDeprivation = chargedWithDeprivation,
                FairlyDescribedAsStateActor = new StateAction2.TestIsStateActor(chargedWithDeprivation)
                {
                    //court concludes this is not a traditional government function
                    IsTraditionalGovernmentFunction = a => !(a is OperationOfPublicAccessCableChannels)
                }

            };

            var testResult = testSubject.IsValid(new ManhattanCommunityAccessCorporation(), new Halleck());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class OperationOfPublicAccessCableChannels : StateAction2 {  }

    public class ManhattanCommunityAccessCorporation : LegalPerson, IPlaintiff
    {
        public ManhattanCommunityAccessCorporation(): base("Manhattan Community Access Corporation") { }
    }

    public class Halleck : LegalPerson, IDefendant
    {
        public Halleck(): base("Halleck") { }
    }
}
