using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var testSubject = new Orbital();
            Console.WriteLine(testSubject.ToString());

            testSubject.SpinUp.IsPresent = true;
            Console.WriteLine(testSubject.ToString());

            testSubject.SpinDown.IsPresent = true;
            Console.WriteLine(testSubject.ToString());
        }
    }
}
