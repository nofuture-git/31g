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

        [Test]
        public void TestIsAlkene()
        {
            var testSubject = MoleculeFactory.Ethylene();
            var testResult = testSubject.IsAlkene();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.Propane();
            testResult = testSubject.IsAlkene();
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestIsAlkyne()
        {
            var testSubject = MoleculeFactory.Ethyne();
            var testResult = testSubject.IsAlkyne();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.Propane();
            testResult = testSubject.IsAlkyne();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Ethylene();
            testResult = testSubject.IsAlkyne();
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestIsAromatic()
        {
            var testSubject = MoleculeFactory.Benzene();
            var testResult = testSubject.IsArene();
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestIsHalide()
        {
            var testSubject = MoleculeFactory.DDT();
            var testResult = testSubject.IsHalide();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.Morphine();
            testResult = testSubject.IsHalide();
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestIsAlcohol()
        {
            var testSubject = MoleculeFactory.Ethanol();
            var testResult = testSubject.IsAlcohol();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.RubbingAlcohol();
            testResult = testSubject.IsAlcohol();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.Antifreeze();
            testResult = testSubject.IsAlcohol();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.Water();
            testResult = testSubject.IsAlcohol();
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestIsEther()
        {
            var testSubject = MoleculeFactory.DiethylEther();
            var testResult = testSubject.IsEther();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.Water();
            testResult = testSubject.IsEther();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Ethanol();
            testResult = testSubject.IsEther();
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestIsCarbonyl()
        {
            var testSubject = MoleculeFactory.Benzaldehyde();
            var testResult = testSubject.IsCarbonyl();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.Acetone();
            testResult = testSubject.IsCarbonyl();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.Ethanol();
            testResult = testSubject.IsCarbonyl();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Water();
            testResult = testSubject.IsCarbonyl();
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestIsAldehyde()
        {
            var testSubject = MoleculeFactory.Benzaldehyde();
            var testResult = testSubject.IsAldehyde();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.Ethanol();
            testResult = testSubject.IsAldehyde();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Water();
            testResult = testSubject.IsAldehyde();
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestIsKetone()
        {
            var testSubject = MoleculeFactory.Acetone();
            var testResult = testSubject.IsKetone();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.Benzaldehyde();
            testResult = testSubject.IsKetone();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Water();
            testResult = testSubject.IsKetone();
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestIsCarboxylicAcid()
        {
            var testSubject = MoleculeFactory.Vinegar();
            var testResult = testSubject.IsCarboxylicAcid();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.Glycine();
            testResult = testSubject.IsCarboxylicAcid();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.Acetone();
            testResult = testSubject.IsCarboxylicAcid();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Benzaldehyde();
            testResult = testSubject.IsCarboxylicAcid();
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestIsEster()
        {
            var testSubject = MoleculeFactory.PineappleSmell();
            var testResult = testSubject.IsEster();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.AppleSmell();
            testResult = testSubject.IsEster();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.VitaminA();
            testResult = testSubject.IsEster();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Acetone();
            testResult = testSubject.IsEster();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Water();
            testResult = testSubject.IsEster();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Ethanol();
            testResult = testSubject.IsEster();
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestIsAmide()
        {
            var testSubject = MoleculeFactory.Penicillin();
            var testResult = testSubject.IsAmide();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.PineappleSmell();
            testResult = testSubject.IsAmide();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.VitaminA();
            testResult = testSubject.IsEster();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Acetone();
            testResult = testSubject.IsEster();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Water();
            testResult = testSubject.IsEster();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Ethanol();
            testResult = testSubject.IsEster();
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestIsNitrile()
        {
            var testSubject = MoleculeFactory.Acetonitrile();
            var testResult = testSubject.IsNitrile();
            Assert.IsTrue(testResult);

            testSubject = MoleculeFactory.Penicillin();
            testResult = testSubject.IsNitrile();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Acetone();
            testResult = testSubject.IsEster();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Water();
            testResult = testSubject.IsEster();
            Assert.IsFalse(testResult);

            testSubject = MoleculeFactory.Ethanol();
            testResult = testSubject.IsEster();
            Assert.IsFalse(testResult);
        }
    }
}
