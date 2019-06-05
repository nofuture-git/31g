using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Sindell v. Abbott Laboratories, 607 P.2d 924 (Cal. 1980)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, multiple tortfeasers for one plainfiff where some are thought as third parties
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class SindellvAbbottLaboratoriesTests
    {
        [Test]
        public void SindellvAbbottLaboratories()
        {
            var test = new ConcertOfAction(ExtensionMethods.Tortfeasor)
            {
                IsPerformAlongside = (lp, thp) => false,
                IsAssistingWith = (lp, thp) =>
                    lp is AbbottLaboratories && thp is SomeOtherParty && ((SomeOtherParty) thp).IsAwareOfOthersActions,
                IsAwareBreachOfDuty = (lp, thp) =>
                    lp is AbbottLaboratories && thp is SomeOtherParty && ((SomeOtherParty) thp).IsAwareOfOthersActions,
            };
            var testResult = test.IsValid(new Sindell(), new AbbottLaboratories(), new SomeOtherParty());

            Assert.IsFalse(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class Sindell : LegalPerson, IPlaintiff
    {
        public Sindell(): base("Sindell") { }
    }

    public class AbbottLaboratories : LegalPerson, ITortfeasor
    {
        public AbbottLaboratories(): base("Abbott Laboratories") { }
    }

    public class SomeOtherParty : LegalPerson, IThirdParty
    {
        public SomeOtherParty() : base("SOME 3RD PARTY") { }

        public bool IsAwareOfOthersActions { get; set; } = false;
    }
}
