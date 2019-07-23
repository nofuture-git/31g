using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.Property.US.Acquisition;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Property.Tests
{
    /// <summary>
    /// Marengo Cave Co. v. Ross, 10 N.E.2d 917 (Ind. 1937)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, spins around on the open and notorious part when the property is some underground cave
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MarengoCaveCovRossTests
    {
        [Test]
        public void MarengoCaveCovRoss()
        {
            var test = new AdversePossession()
            {
                SubjectProperty = new LegalProperty("part of 'Marengo Cave'")
                {
                    IsEntitledTo = lp => lp is Ross,
                    IsInPossessionOf = lp => lp is MarengoCaveCo
                },
                Consent = Consent.NotGiven(),
                EntitledOwnersAction = Act.DueDiligence(),
                Inception = new DateTime(1908, 1, 1),
                //time the case was written
                Terminus = new DateTime(1937, 1, 1),
                IsContinuousPossession = lp => true,
                IsExclusivePossession = lp => true,
                IsOpenNotoriousPossession = lp => false
            };

            var testResult = test.IsValid(new MarengoCaveCo(), new Ross());
            Assert.IsFalse(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class MarengoCaveCo : LegalPerson, IPlaintiff, IDisseisor
    {
        public MarengoCaveCo(): base("Marengo Cave Co.") { }
    }

    public class Ross : LegalPerson, IDefendant
    {
        public Ross(): base("Ross") { }
    }
}
