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

        [Test]
        public void TestAsDouble()
        {
            var testInput00 = new Oxygen();
            var testInput01 = new Oxygen();
            BondFactory.CreateBond(testInput00, testInput01).AsDoubleBond();
            Assert.AreEqual(0, testInput00.CountValences);
            Assert.AreEqual(0, testInput01.CountValences);

        }

        [Test]
        public void TestPolarCovalent()
        {
            var testInput00 = new Oxygen();
            var testInput01 = new Hydrogen();

            var testOutput = BondFactory.CreateBond(testInput00, testInput01).GetBond<PolarCovalent>();
            Assert.IsNotNull(testOutput);
            var dipole = testOutput.DipoleVectorSize;
            Assert.IsTrue(dipole > 0D);

            Console.WriteLine(dipole);
        }
    }
}
