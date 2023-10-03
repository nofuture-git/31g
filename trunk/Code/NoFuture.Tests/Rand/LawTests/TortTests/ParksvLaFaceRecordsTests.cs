using System;
using NUnit.Framework;
using NoFuture.Law.US;
using NoFuture.Law.Tort.US.IntentionalTort;
using NoFuture.Law.US.Persons;

namespace NoFuture.Law.Tort.Tests
{
    /// <summary>
    /// Parks v. La Face Records, 76 F. Supp. 2d 775 (E.D. Mich. 1999).
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue,  personality identity use for artistic expression is not
    /// protected even when such expression is promoted for commercial reasons
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ParksvLaFaceRecordsTests
    {
        [Test]
        public void ParksvLaFaceRecords()
        {
            var test = new FalseEndorsement(ExtensionMethods.Defendant)
            {
                IsCommercialUse = lp => lp is LaFaceRecords,
                IsAppropriatedPersonIdentity = (l1, l2) => l1 is Parks && l2 is LaFaceRecords,
                IsFirstAmendmentProtected = lp => lp is LaFaceRecords
            };

            var testResult = test.IsValid(new Parks(), new LaFaceRecords());
            Assert.IsFalse(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class Parks : LegalPerson, IPlaintiff
    {
        public Parks(): base("Rosa Parks") { }
    }

    public class LaFaceRecords : LegalPerson, ITortfeasor
    {
        public LaFaceRecords(): base("La Face Records") { }
    }
}
