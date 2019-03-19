using System;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Trespass;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.TrespassTests
{
    /// <summary>
    /// Butler v. Florida, No. 1D08-0958 (Fla: Dist. Court of Appeals, 2009).
    /// </summary>
    /// <remarks>
    ///<![CDATA[
    /// doctrine issue, predicate tests of the Necessity defense
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class ButlervFloridaTests
    {
        [Test]
        public void ButlervFlorida()
        {
            var testCrime = new Felony
            {
                ActusReus = new CriminalTrespass
                {
                    IsEntry = lp => lp is Butler,
                    SubjectProperty = new LegalProperty("Thelma Harvey house")
                    {
                        EntitledTo = new ThelmaHarvey(),
                        InPossessionOf = new ThelmaHarvey()
                    }
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is Butler
                }
            };

            var testResult = testCrime.IsValid(new Butler(), new ThelmaHarvey());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);

            var testDefense = new NecessityDefense(testCrime)
            {
                //according to Butler - was being chased
                IsMultipleInHarm = lp => lp is Butler,

                //no one else saw the defendant being chased 
                Imminence = new Imminence(ExtensionMethods.Defendant)
                {
                    IsImmediatePresent = ts => false,
                },
                Proportionality = new ChoiceThereof<ITermCategory>(ExtensionMethods.Defendant)
                {
                    IsProportional = (t1, t2) => false
                },
                //court finds that he went to a party were he knew this may happen
                IsResponsibleForSituationArise = lp => lp is Butler
            };

            testResult = testDefense.IsValid(new Butler(), new ThelmaHarvey());
            Console.WriteLine(testDefense.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Butler : LegalPerson, IDefendant
    {
        public Butler(): base("CAS SIUS BUTLER") {  }
    }

    public class ThelmaHarvey : LegalPerson, IVictim
    {
        public ThelmaHarvey() : base ("THELMA HARVEY") {  }
    }
}
