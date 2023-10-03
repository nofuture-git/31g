using System;
using NUnit.Framework;
using NoFuture.Law.US;
using NoFuture.Law.Tort.US.IntentionalTort;
using NoFuture.Law.US.Persons;

namespace NoFuture.Law.Tort.Tests
{
    /// <summary>
    /// Midler v. Ford Motor Company, 849 F.2d 460 (9th. Cir. 1986).
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, personality identity need not be specific picture or name but enought to recognize
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MidlervFordMotorCompanyTests
    {
        [Test]
        public void MidlervFordMotorCompany()
        {
            var test = new FalseEndorsement(ExtensionMethods.Defendant)
            {
                IsFirstAmendmentProtected = lp => false,
                IsCommercialUse = lp => lp is FordMotorCompany,
                IsAppropriatedPersonIdentity = (l1, l2) => l1 is Midler && l2 is FordMotorCompany
            };
            var testResult = test.IsValid(new Midler(), new FordMotorCompany());
            Assert.IsTrue(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class Midler : LegalPerson, IPlaintiff
    {
        public Midler(): base("Bette Midler") { }
    }

    public class FordMotorCompany : LegalPerson, ITortfeasor
    {
        public FordMotorCompany(): base("Ford Motor Company") { }
    }
}
