using System;
using NUnit.Framework;
using NoFuture.Law.US.Persons;
using NoFuture.Law.Tort.US.ReasonableCare;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.Tests
{
    /// <summary>
    /// Breunig v. American Family Ins. Co., 173 N.W.2d 619 (Wis. 1970)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class BreunigvAmericanFamilyInsCoTests
    {
        [Test]
        public void BreunigvAmericanFamilyInsCo()
        {
            var test = new OfMentalCapacity(ExtensionMethods.Tortfeasor)
            {
                IsMentallyIncapacitated = lp => lp is ErmaVeith,
                IsIncapacityForeseeable = lp => !(lp is ErmaVeith)
            };

            var testResult = test.IsValid(new AmericanFamilyInsCo(), new Breunig());
            Assert.IsTrue(testResult);
            Console.WriteLine(test.ToString());

        }
    }

    public class Breunig : LegalPerson, IPlaintiff
    {
        public Breunig(): base("Breunig") { }
    }

    public class AmericanFamilyInsCo : ErmaVeith
    {
        public AmericanFamilyInsCo() : this("American Family Ins. Co.,") { }
        public AmericanFamilyInsCo(string name) : base (name) { }
    }

    public class ErmaVeith : LegalPerson, ITortfeasor
    {
        public ErmaVeith(): base("Erma Veith") { }
        public ErmaVeith(string name) :base(name) { }
    }
}
