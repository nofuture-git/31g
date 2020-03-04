using System;
using System.Linq;
using NCDK.Default;
using NUnit.Framework;
using Notes.Chemistry.Cdk;
using MoleculeFactory = Notes.Chemistry.Cdk.MoleculeFactory;

namespace Test.Notes.Chemistry
{
    [TestFixture]
    public class TestCdkExtensions
    {
        [Test]
        public void TestGetCountMolecules()
        {
            var testSubject = MoleculeFactory.Propane();

            var testResult = testSubject.GetCountMolecules(74.6D);
            Console.WriteLine(string.Format("{0:n0}", testResult));
            
            Assert.IsTrue(testResult > 1.018E+24);
            Assert.IsTrue(testResult < 1.019E+24);

        }

        [Test]
        public void TestElectronegativity()
        {
            var testSubject = new Atom("O");
            var testResult = testSubject.Electronegativity();
            Assert.IsTrue(testResult > 0D);
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestGetMoreElectronegativeAtom()
        {
            var testInput00 = new Atom("O");
            var testInput01 = new Atom("H");

            var testSubject = new Bond(testInput00, testInput01);

            var testResult = testSubject.GetMostElectronegativeAtom();
            Assert.AreEqual(testInput00, testResult);
        }

        [Test]
        public void TestGetMostElectronegativeAtom()
        {
            var testSubject = MoleculeFactory.Water();

            var testResult = testSubject.GetMostElectronegativeAtom();
            Assert.IsNotNull(testResult);

            var testCtrl = testSubject.Atoms.FirstOrDefault(a => a.Symbol == "O");
            Assert.IsNotNull(testCtrl);

            Assert.AreEqual(testCtrl, testResult);
        }

        [Test]
        public void TestValenceCount()
        {
            var testSubject = new Atom("O");
            var testResult = testSubject.ValenceCount();
            Assert.AreEqual(2, testResult);
        }

        [Test]
        public void TestAddFormalCharges()
        {
            var testSubject = MoleculeFactory.Hydronium();
            testSubject.SumFormalCharge();
            var testResult = testSubject.Atoms.FirstOrDefault(a => a.Symbol == "O");
            Assert.IsNotNull(testResult);
            Assert.AreEqual(1, testResult.FormalCharge);

            var pathToImg = testSubject.DepictMolecule();
            Console.WriteLine(pathToImg);
        }
    }
}
