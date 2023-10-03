using System;
using NUnit.Framework;
using NoFuture.Law.US;
using NoFuture.Law.Tort.US.Elements;
using NoFuture.Law.Tort.US.IntentionalTort;
using NoFuture.Law.US.Persons;

namespace NoFuture.Law.Tort.Tests
{
    /// <summary>
    /// Desnick v. American Broadcasting Companies, Inc., 44 F.3d 1345 (7th Cir. 1995).
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, trespass requires some protectable interest
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class DesnickvAmericanBroadcastingCompaniesIncTests
    {
        [Test]
        public void DesnickvAmericanBroadcastingCompaniesInc()
        {
            var test = new TrespassToPerson(ExtensionMethods.Tortfeasor)
            {
                Consent = Consent.NotGiven(),
                Causation = Causation.ItsObvious(),
                SubjectProperty = new LegalProperty("some office"),
                Injury = new Defamation(ExtensionMethods.Tortfeasor)
                {
                    IsFalseStatement = (l1, l2) => l1 is Desnick && l2 is AmericanBroadcastingCompaniesInc,
                    IsPublishedStatement = lp => lp is AmericanBroadcastingCompaniesInc,
                    IsUnwantedStatement = lp => lp is Desnick
                },
            };

            var testResult = test.IsValid(new Desnick(), new AmericanBroadcastingCompaniesInc());
            Assert.IsTrue(testResult);


            test.Injury = new InvasionOfPrivacy(ExtensionMethods.Tortfeasor)
            {
                IsInPlainSight = lp => true,
                IsExpectationOfPrivacy = lp => false
            };

            testResult = test.IsValid(new Desnick(), new AmericanBroadcastingCompaniesInc());
            Assert.IsFalse(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class Desnick : LegalPerson, IPlaintiff
    {
        public Desnick(): base("Desnick") { }
    }

    public class AmericanBroadcastingCompaniesInc : LegalPerson, ITortfeasor
    {
        public AmericanBroadcastingCompaniesInc(): base("American Broadcasting Companies, Inc.") { }
    }
}
