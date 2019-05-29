using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements.ReasonableCare;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Levi v. Sw. La. Elec. Membership Coop., 542 So. 2d 1081, 1084 (La. 1989)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, being an expert alone does not raise the level of reasonable care, it must be distinct\dangerous or relied upon
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class LevivSWLaElecMembershipCoOpTests
    {
        [Test]
        public void LevivSWLaElecMembershipCoOp()
        {
            var test = new OfExpert<PowerLines>(ExtensionMethods.Tortfeasor)
            {
                IsSignificantDanger = lp => lp is SWLaElecMembershipCoOp,
                IsExercisedExpertCare = lp => false
            };

            var testResult = test.IsValid(new Levi(), new SWLaElecMembershipCoOp());
            Assert.IsFalse(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class Levi : LegalPerson, IPlaintiff
    {
        public Levi(): base("Levi") { }
    }

    public class SWLaElecMembershipCoOp : LegalPerson, ITortfeasor, IExpert<PowerLines>
    {
        public SWLaElecMembershipCoOp(): base("SW La. Elec. Membership Co-Op.") { }
        public Predicate<PowerLines> IsSkilledOrKnowledgeableOf { get; set; } = r => true;
    }

    public class PowerLines : LegalProperty
    {

    }
}
