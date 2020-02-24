using System;
using Notes.Chemistry.Cdk;
using NUnit.Framework;

namespace Test.Notes.Chemistry
{
    [TestFixture]
    public class TestMoleculeFactory
    {
        [Test]
        public void TestPropane()
        {
            var propane = MoleculeFactory.Propane();
            var pathToImg = propane.DepictMolecule();

            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestEthylene()
        {
            var ethylene = MoleculeFactory.Ethylene();
            var pathToImg = ethylene.DepictMolecule();

            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestEthyne()
        {
            var ethyne = MoleculeFactory.Ethyne();
            var pathToImg = ethyne.DepictMolecule();

            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestCyclopentene()
        {
            var cyclopentene = MoleculeFactory.Cyclopentene();
            var pathToImg = cyclopentene.DepictMolecule();

            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestVitaminA()
        {
            var vitaminA = MoleculeFactory.VitaminA();
            var pathToImg = vitaminA.DepictMolecule();

            Console.WriteLine(pathToImg);
        }
    }
}
