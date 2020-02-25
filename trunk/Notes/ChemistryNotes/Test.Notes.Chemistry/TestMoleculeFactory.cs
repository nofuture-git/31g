﻿using System;
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

        [Test]
        public void TestBenzene()
        {
            var benzene = MoleculeFactory.Benzene();
            var pathToImg = benzene.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestMorphine()
        {
            var morphine = MoleculeFactory.Morphine();
            var pathToImg = morphine.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestTrichlorofluoromethane()
        {
            var trichl = MoleculeFactory.Trichlorofluoromethane();
            var pathToImg = trichl.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestRefrigerant()
        {
            var refrigerant = MoleculeFactory.Refrigerant();
            var pathToImg = refrigerant.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestDDT()
        {
            var ddt = MoleculeFactory.DDT();
            var pathToImg = ddt.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestEthanol()
        {
            var ethanol = MoleculeFactory.Ethanol();
            var pathToImg = ethanol.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestRubbingAlcohol()
        {
            var rubbingAlcohol = MoleculeFactory.RubbingAlcohol();
            var pathToImg = rubbingAlcohol.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestWater()
        {
            var water = MoleculeFactory.Water();
            var pathToImg = water.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestAntifreeze()
        {
            var antifreeze = MoleculeFactory.Antifreeze();
            var pathToImg = antifreeze.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestFructose()
        {
            var fructose = MoleculeFactory.Fructose();
            var pathToImg = fructose.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestGlucose()
        {
            var glucose = MoleculeFactory.Glucose();
            var pathToImg = glucose.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestSucrose()
        {
            var sucrose = MoleculeFactory.Sucrose();
            var pathToImg = sucrose.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestDiethylEther()
        {
            var ether = MoleculeFactory.DiethylEther();
            var pathToImg = ether.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestBenzaldehyde()
        {
            var benzaldehyde = MoleculeFactory.Benzaldehyde();
            var pathToImg = benzaldehyde.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestAcetone()
        {
            var acetone = MoleculeFactory.Acetone();
            var pathToImg = acetone.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestVinegar()
        {
            var vinegar = MoleculeFactory.Vinegar();
            var pathToImg = vinegar.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestGlycine()
        {
            var glycine = MoleculeFactory.Glycine();
            var pathToImg = glycine.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestPineappleSmell()
        {
            var pineappleSmell = MoleculeFactory.PineappleSmell();
            var pathToImg = pineappleSmell.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestAppleSmell()
        {
            var appleSmell = MoleculeFactory.AppleSmell();
            var pathToImg = appleSmell.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestPenicillin()
        {
            var penicillin = MoleculeFactory.Penicillin();
            var pathToImg = penicillin.DepictMolecule();
            Console.WriteLine(pathToImg);
        }

        [Test]
        public void TestAcetonitrile()
        {
            var acetonitrile = MoleculeFactory.Acetonitrile();
            var pathToImg = acetonitrile.DepictMolecule();
            Console.WriteLine(pathToImg);
        }
    }
}
