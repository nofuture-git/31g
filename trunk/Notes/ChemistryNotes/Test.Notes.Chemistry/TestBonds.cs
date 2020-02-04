using Notes.Chemistry.Elements;
using Notes.Chemistry.Elements.Bonds;
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

            Assert.IsTrue(testResult is PurelyCovalent);

            testInput00 = new Hydrogen();
            testInput01 = new Chlorine();

            testResult = BondFactory.CreateBond(testInput00, testInput01);
            Assert.IsTrue(testResult is PolarCovalent);
        }
    }
}
