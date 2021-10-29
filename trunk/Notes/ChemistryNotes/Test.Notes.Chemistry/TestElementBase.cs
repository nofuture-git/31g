using System;
using System.Linq;
using Notes.Chemistry.Elements;
using Notes.Chemistry.Elements.ElectronCfg.Shells;
using Notes.Chemistry.Elements.PeriodicTable;
using NUnit.Framework;

namespace Test.Notes.Chemistry
{
    [TestFixture]
    public class TestElementBase
    {
        [Test]
        public void TestName()
        {
            IElement testSubject = new Carbon();
            Console.WriteLine(testSubject.Name);
        }

        [Test]
        public void TestShellMaxElectrons()
        {
            IElement testSubject = new Carbon();
            Console.WriteLine(testSubject.MaxElectrons);
            Assert.AreEqual(10, testSubject.MaxElectrons);
            
        }

        [Test]
        public void TestPrintElectronShellCfg()
        {
            IElement testSubject = new Carbon();
            var testResult = testSubject.PrintElectronShellCfg(false);
            Console.WriteLine(testResult);
            Assert.AreEqual("1s2 2s2 2p_x^1 2p_y^1 2p_z^0", testResult);

            testResult = testSubject.PrintElectronShellCfg();
            Assert.AreEqual("1s2 2s2 2p2", testResult);
            Console.WriteLine(testResult);

            Console.WriteLine(new Sodium().PrintElectronShellCfg());
            
        }

        [Test]
        public void TestPrintOrbitalToString()
        {
            IElement testSubject = new Carbon();
            foreach (var shell in testSubject.Shells)
            {
                foreach (var orbitalGrp in shell.Orbitals)
                {
                    Console.WriteLine($"{shell.GetType().Name} {orbitalGrp.GetType().Name}");
                    foreach (var orbital in orbitalGrp.AssignedElectrons)
                    {
                        Console.Write(orbital.ToString());
                    }
                }
                Console.WriteLine();
            }
        }

        [Test]
        public void TestCountValences()
        {
            IElement testSubject = new Hydrogen();
            var testResult = testSubject.CountValences;
            Console.WriteLine($"{testSubject.Name} {testResult}");
            Assert.AreEqual(1,testResult);

            testSubject = new Helium();
            testResult = testSubject.CountValences;
            Console.WriteLine($"{testSubject.Name} {testResult}" );
            Assert.AreEqual(0, testResult);

            testSubject = new Oxygen();
            testResult = testSubject.CountValences;
            Console.WriteLine($"{testSubject.Name} {testResult}");
            Assert.AreEqual(2, testResult);

            testSubject = new Nitrogen();
            testResult = testSubject.CountValences;
            Console.WriteLine($"{testSubject.Name} {testResult}");
            Assert.AreEqual(3, testResult);

            testSubject = new Carbon();
            testResult = testSubject.CountValences;
            Console.WriteLine($"{testSubject.Name} {testResult}");
            Assert.AreEqual(4, testResult);

            testSubject = new Silicon();
            testResult = testSubject.CountValences;
            Console.WriteLine($"{testSubject.Name} {testResult}");

        }

        [Test]
        public void TestProperties()
        {
            IElement testSubject = new Carbon();
            Assert.IsTrue(testSubject.AtomicNumber > 0);
            Assert.IsNotNull(testSubject.Symbol);
            Assert.AreNotEqual(0D, testSubject.AtomicMass);
            Console.WriteLine(testSubject.RoomTempPhase);
            Assert.IsFalse(testSubject.IsRadioactive);
            Assert.IsFalse(testSubject.IsArtificial);
        }

        [Test]
        public void TestPrintElectronShellCfg_All()
        {
            IElement testSubject; ;
            testSubject = new Hydrogen();
            Console.WriteLine($"Hydrogen {testSubject.PrintElectronShellCfg()}");
            testSubject = new Helium();
            Console.WriteLine($"Helium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Lithium();
            Console.WriteLine($"Lithium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Beryllium();
            Console.WriteLine($"Beryllium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Boron();
            Console.WriteLine($"Boron {testSubject.PrintElectronShellCfg()}");
            testSubject = new Carbon();
            Console.WriteLine($"Carbon {testSubject.PrintElectronShellCfg()}");
            testSubject = new Nitrogen();
            Console.WriteLine($"Nitrogen {testSubject.PrintElectronShellCfg()}");
            testSubject = new Oxygen();
            Console.WriteLine($"Oxygen {testSubject.PrintElectronShellCfg()}");
            testSubject = new Fluorine();
            Console.WriteLine($"Fluorine {testSubject.PrintElectronShellCfg()}");
            testSubject = new Neon();
            Console.WriteLine($"Neon {testSubject.PrintElectronShellCfg()}");
            testSubject = new Sodium();
            Console.WriteLine($"Sodium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Magnesium();
            Console.WriteLine($"Magnesium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Aluminum();
            Console.WriteLine($"Aluminum {testSubject.PrintElectronShellCfg()}");
            testSubject = new Silicon();
            Console.WriteLine($"Silicon {testSubject.PrintElectronShellCfg()}");
            testSubject = new Phosphorus();
            Console.WriteLine($"Phosphorus {testSubject.PrintElectronShellCfg()}");
            testSubject = new Sulfur();
            Console.WriteLine($"Sulfur {testSubject.PrintElectronShellCfg()}");
            testSubject = new Chlorine();
            Console.WriteLine($"Chlorine {testSubject.PrintElectronShellCfg()}");
            testSubject = new Argon();
            Console.WriteLine($"Argon {testSubject.PrintElectronShellCfg()}");
            testSubject = new Potassium();
            Console.WriteLine($"Potassium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Calcium();
            Console.WriteLine($"Calcium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Scandium();
            Console.WriteLine($"Scandium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Titanium();
            Console.WriteLine($"Titanium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Vanadium();
            Console.WriteLine($"Vanadium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Chromium();
            Console.WriteLine($"Chromium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Manganese();
            Console.WriteLine($"Manganese {testSubject.PrintElectronShellCfg()}");
            testSubject = new Iron();
            Console.WriteLine($"Iron {testSubject.PrintElectronShellCfg()}");
            testSubject = new Cobalt();
            Console.WriteLine($"Cobalt {testSubject.PrintElectronShellCfg()}");
            testSubject = new Nickel();
            Console.WriteLine($"Nickel {testSubject.PrintElectronShellCfg()}");
            testSubject = new Copper();
            Console.WriteLine($"Copper {testSubject.PrintElectronShellCfg()}");
            testSubject = new Zinc();
            Console.WriteLine($"Zinc {testSubject.PrintElectronShellCfg()}");
            testSubject = new Gallium();
            Console.WriteLine($"Gallium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Germanium();
            Console.WriteLine($"Germanium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Arsenic();
            Console.WriteLine($"Arsenic {testSubject.PrintElectronShellCfg()}");
            testSubject = new Selenium();
            Console.WriteLine($"Selenium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Bromine();
            Console.WriteLine($"Bromine {testSubject.PrintElectronShellCfg()}");
            testSubject = new Krypton();
            Console.WriteLine($"Krypton {testSubject.PrintElectronShellCfg()}");
            testSubject = new Rubidium();
            Console.WriteLine($"Rubidium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Strontium();
            Console.WriteLine($"Strontium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Yttrium();
            Console.WriteLine($"Yttrium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Zirconium();
            Console.WriteLine($"Zirconium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Niobium();
            Console.WriteLine($"Niobium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Molybdenum();
            Console.WriteLine($"Molybdenum {testSubject.PrintElectronShellCfg()}");
            testSubject = new Technetium();
            Console.WriteLine($"Technetium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Ruthenium();
            Console.WriteLine($"Ruthenium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Rhodium();
            Console.WriteLine($"Rhodium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Palladium();
            Console.WriteLine($"Palladium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Silver();
            Console.WriteLine($"Silver {testSubject.PrintElectronShellCfg()}");
            testSubject = new Cadmium();
            Console.WriteLine($"Cadmium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Indium();
            Console.WriteLine($"Indium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Tin();
            Console.WriteLine($"Tin {testSubject.PrintElectronShellCfg()}");
            testSubject = new Antimony();
            Console.WriteLine($"Antimony {testSubject.PrintElectronShellCfg()}");
            testSubject = new Tellurium();
            Console.WriteLine($"Tellurium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Iodine();
            Console.WriteLine($"Iodine {testSubject.PrintElectronShellCfg()}");
            testSubject = new Xenon();
            Console.WriteLine($"Xenon {testSubject.PrintElectronShellCfg()}");
            testSubject = new Cesium();
            Console.WriteLine($"Cesium {testSubject.PrintElectronShellCfg()}");
            testSubject = new Barium();
            Console.WriteLine($"Barium {testSubject.PrintElectronShellCfg()}");
        }

        [Test]
        public void TestGetGrams()
        {
            IElement testSubject = new Iron();
            var testResult = testSubject.GetGrams(600);

            Assert.IsTrue(testResult > 5.56E-20 && testResult < 5.57E-20);

            Console.WriteLine(testResult);
        }

        [Test]
        public void TestGetAtoms()
        {
            IElement testSubject = new Carbon();
            var testResult = testSubject.GetAtoms(0.5);
            Console.WriteLine(testResult);

            Assert.IsTrue(testResult > 2.5E+22 && testResult < 2.6E+22);
        }

        [Test]
        public void TestCountElectrons()
        {
            IElement testSubject = new Carbon();

            var testResult = testSubject.CountElectrons;
            Console.WriteLine(testResult);
            Assert.AreEqual(6, testResult);
        }

        [Test]
        public void TestIsCation()
        {
            IElement testSubject = new Carbon();
            Assert.IsFalse(testSubject.IsCation);
            testSubject.RemoveElectron();
            Assert.IsTrue(testSubject.IsCation);
        }

        [Test]
        public void TestIsAnion()
        {
            IElement testSubject = new Carbon();
            Assert.IsFalse(testSubject.IsAnion);
            testSubject.AddElectron();
            Assert.IsTrue(testSubject.IsAnion);

        }

        [Test]
        public void TestIsIon()
        {
            IElement testSubject = new Carbon();
            Assert.IsFalse(testSubject.IsIon);
            testSubject.AddElectron();
            Assert.IsTrue(testSubject.IsIon);
        }

        [Test]
        public void TestEquals()
        {
            IElement testSubject = new Carbon();
            //this should do nothing 
            Assert.AreEqual(1, testSubject.Shells.Count(s => s is KShell));

            testSubject.Shells.Add(new KShell(testSubject));

            Assert.AreEqual(1, testSubject.Shells.Count(s => s is KShell));
        }

        [Test]
        public void TestValenceShell()
        {
            IElement testSubject = new Carbon();

            var testResult = testSubject.ValenceShell;
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult is LShell);
            Assert.AreEqual(4, testResult.GetCountElectrons());
        }

        [Test]
        public void TestHybridizeOrbits()
        {
            var carbon = new Carbon();
            carbon.HybridizeOrbits(4);
            Assert.AreEqual(109.5D, carbon.GetBondAngle());
            Assert.AreEqual("Tetrahedral", carbon.GetBondGeometry());
            Console.WriteLine(carbon.PrintElectronShellCfg(false));

            carbon = new Carbon();
            carbon.HybridizeOrbits(3);
            Assert.AreEqual(120D, carbon.GetBondAngle());
            Assert.AreEqual("Trigonal Planar", carbon.GetBondGeometry());
            Console.WriteLine(carbon.PrintElectronShellCfg(false));

            carbon = new Carbon();
            carbon.HybridizeOrbits(2);
            Assert.AreEqual(180D, carbon.GetBondAngle());
            Assert.AreEqual("Linear", carbon.GetBondGeometry());
            Console.WriteLine(carbon.PrintElectronShellCfg(false));

        }
    }
    
}
