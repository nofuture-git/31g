using System;
using NUnit.Framework;
using NoFuture.Law.US.Persons;
using NoFuture.Law.Tort.US.Elements;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.Tests
{
    /// <summary>
    /// Ira S. Bushey & Sons, Inc. v. United States, 398 F.2d 167 (2d Cir. 1968)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, illustrate vicarious Liability
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class IraSBusheySonsIncvUnitedStatesTests
    {
        [Test]
        public void IraSBusheySonsIncvUnitedStates()
        {
            var test = new EmployeeVicariousLiability(ExtensionMethods.Tortfeasor)
            {
                IsMutuallyBeneficialRelationship = (er, ee) => er is UnitedStatesAgain && ee is SeamanLane,
                IsActInScopeOfEmployment = lp => lp is SeamanLane,
                IsVoluntary = lp => lp is SeamanLane,
                IsAction = lp => lp is SeamanLane
            };

            var testResult = test.IsValid(new IraSBusheySonsInc(), new UnitedStatesAgain(), new SeamanLane());
            Console.WriteLine(test.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class IraSBusheySonsInc : LegalPerson, IPlaintiff
    {
        public IraSBusheySonsInc(): base("Ira S. Bushey & Sons, Inc") { }
    }

    public class UnitedStatesAgain : LegalPerson, IEmployer
    {
        public UnitedStatesAgain() :base("United States") { }
    }

    public class SeamanLane : LegalPerson, ITortfeasor, IEmployee
    {
        public SeamanLane():base("Seaman Lane") { }
    }
    
}
