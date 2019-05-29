using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.Tort.US.Elements.ReasonableCare;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
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
