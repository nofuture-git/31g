using NCDK;
using NCDK.Default;
using Notes.Chemistry.Cdk;
using NUnit.Framework;

namespace Test.Notes.Chemistry
{
    [TestFixture]
    public class TestFunctionalGroup
    {
        [Test]
        public void TestIsHydrocarbon()
        {
            var propane = MoleculeFactory.Propane();
            var testResult = propane.IsHydrocarbon();
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestIsAlkane()
        {
            var testSubject = MoleculeFactory.Propane();
            var testResult = testSubject.IsAlkane();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.VitaminA();
            testResult = testSubject.IsAlkane();
            Assert.IsFalse(testResult);

        }
    }
}
