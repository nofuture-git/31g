using System;
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

        }
    }
}
