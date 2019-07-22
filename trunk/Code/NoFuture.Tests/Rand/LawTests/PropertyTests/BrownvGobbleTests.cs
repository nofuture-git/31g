using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Property.US.Acquisition;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    /// <summary>
    /// Brown v. Gobble, 474 S.E.2d 489 (W. Va. 1996)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, example of adverse possession with the concept of "tracking" (where prior owners already satisfied adverse possession)
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class BrownvGobbleTests
    {
        [Test]
        public void BrownvGobble()
        {
            var test = new AdversePossession(ExtensionMethods.Disseisor)
            {
                SubjectProperty = new TwoFeetWideTract
                {
                    EntitledTo = new Gobble(),
                    InPossessionOf = new Brown(),
                },
                Consent = Consent.NotGiven(),
                OpenNotoriousUse = Act.ItsClearAndObvious(),
                IsExclusiveUse = p => p is TwoFeetWideTract,
                IsContinuousUse = p => p is TwoFeetWideTract,
            };

            var testResult = test.IsValid(new Brown(), new Gobble());
            Console.WriteLine(test.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class TwoFeetWideTract : LegalProperty
    {
        public TwoFeetWideTract(): base("two-feet-wide tract") { }
    }

    public class Brown : LegalPerson, IPlaintiff, IDisseisor
    {
        public Brown(): base("Brown") { }
    }

    public class Gobble : LegalPerson, IDefendant
    {
        public Gobble(): base("Gobble") { }
    }
}
