using System;
using Notes.Chemistry.Elements;
using Notes.Chemistry.Elements.ElectronCfg.Orbitals;
using NUnit.Framework;

namespace Test.Notes.Chemistry
{
    [TestFixture]
    public class TestOrbital
    {
        [Test]
        public void TestToString()
        {
            var testSubject = new Orbital(new StubIOrbital());
            Console.WriteLine(testSubject.ToString());

            testSubject.SpinUp.IsPresent = true;
            Console.WriteLine(testSubject.ToString());

            testSubject.SpinDown.IsPresent = true;
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class StubIOrbital : IOrbitalGroup
    {
        public int CompareTo(IOrbitalGroup other)
        {
            return -1;
        }

        public IShell Shell { get; }
        public int? AddElectron()
        {
            return null;
        }

        public int? RemoveElectron()
        {
            return null;
        }

        public int GetCountElectrons()
        {
            return 0;
        }

        public Orbital[] AssignedElectrons { get; }
    }
}
