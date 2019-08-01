using System;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    /// <summary>
    /// Rucker v. Wynn, 212 Ga. App. 69 (Ga. Ct. App. 1994)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, commerical lease are in favor of landlord
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class RuckervWynnTests
    {
        [Test]
        public void RuckervWynn()
        {
            var lease = new Leasehold
            {
                SubjectProperty = new RealProperty("premises for a restaurant business")
                {
                    IsEntitledTo = lp => lp is Rucker
                },
                Inception = new DateTime(1990, 7, 1),
                Terminus = new DateTime(1995, 7, 1)
            };

            var testResult = lease.IsValid(new Rucker(), new Wynn());
            Assert.IsTrue(testResult);

            var test = new Eviction(lease)
            {
                CurrentDateTime = new DateTime(1994, 1, 1),
                //late rent
                IsBreachLeaseCondition = lp => lp is Wynn,
                IsResidenceHome = p => false,
                IsPeaceableSelfHelpReentry = lp => lp is Rucker
            };

            testResult = test.IsValid(new Rucker(), new Wynn());
            Console.WriteLine(test.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Rucker : LegalPerson, IPlaintiff, ILessor
    {
        public Rucker(): base("Rucker") { }
    }

    public class Wynn : LegalPerson, IDefendant, ILessee
    {
        public Wynn(): base("Wynn") { }
    }
}
