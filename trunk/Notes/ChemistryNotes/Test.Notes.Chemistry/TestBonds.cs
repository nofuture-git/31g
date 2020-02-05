using System;
using Notes.Chemistry.Elements;
using Notes.Chemistry.Elements.Bonds;
using Notes.Chemistry.Elements.Bonds.KindsOf;
using Notes.Chemistry.Elements.PeriodicTable;
using NUnit.Framework;

namespace Test.Notes.Chemistry
{
    [TestFixture]
    public class TestBonds
    {
        [Test]
        public void TestBondFactory()
        {
            IElement testInput00 = new Sodium();
            IElement testInput01 = new Chlorine();
            var testResult = BondFactory.CreateBond(testInput00, testInput01);
            Assert.IsTrue(testResult is Ionic);

            testInput00 = new Hydrogen();
            testInput01 = new Hydrogen();

            testResult = BondFactory.CreateBond(testInput00, testInput01);
            
            Assert.IsTrue(testResult.Is(typeof(PurelyCovalent)));

            testInput00 = new Hydrogen();
            testInput01 = new Chlorine();

            testResult = BondFactory.CreateBond(testInput00, testInput01);
            Assert.IsTrue(testResult.Is(typeof( PolarCovalent)));
        }

        [Test]
        public void TestGetBond()
        {
            var testResult = BondFactory.CreateBond(new Hydrogen(), new Chlorine()).GetBond<PolarCovalent>();
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult.DeltaPlus?.Name);
            Console.WriteLine(testResult.DeltaMinus?.Name);
        }
    }
}
