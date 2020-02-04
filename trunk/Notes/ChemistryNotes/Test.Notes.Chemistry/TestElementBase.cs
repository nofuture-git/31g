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
            Console.WriteLine(carbon.PrintElectronShellCfg(false));

            carbon = new Carbon();
            carbon.HybridizeOrbits(3);
            Console.WriteLine(carbon.PrintElectronShellCfg(false));

            carbon = new Carbon();
            carbon.HybridizeOrbits(2);
            Console.WriteLine(carbon.PrintElectronShellCfg(false));

        }
    }
    
}
