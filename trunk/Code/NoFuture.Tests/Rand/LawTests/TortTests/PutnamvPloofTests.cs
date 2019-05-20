using System;
using NoFuture.Rand.Law.Criminal.US.Terms.Violence;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Ploof v. Putnam, 71 A. 188 (Vt. 1908)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctine issue, necessity defense again 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class PutnamvPloofTests
    {
        [Test]
        public void PutnamvPloof()
        {
            var test =
                new NecessityDefense<ITermCategory>(ExtensionMethods.Tortfeasor)
                {
                    IsMultipleInHarm = lp => true,
                    IsResponsibleForSituationArise = lp => false,
                    Proportionality = new ChoiceThereof<ITermCategory>(ExtensionMethods.Tortfeasor)
                    {
                        GetChoice = lp => new MoorTheSloop(),
                        GetOtherPossibleChoices = lp => new ITermCategory[] {new SeriousBodilyInjury(),},
                    }
                };
            var testResult = test.IsValid(new Putnam(), new Ploof());
            Console.WriteLine(test.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class MoorTheSloop : TermCategory
    {
        protected override string CategoryName => "moor the sloop to defendant’s dock";
    }

    public class Putnam : LegalPerson, ITortfeasor
    {
        public Putnam(): base("") { }
    }

    public class Ploof : LegalPerson, IPlaintiff
    {
        public Ploof(): base("") { }
    }
}
