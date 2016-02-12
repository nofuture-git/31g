using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;
using NoFuture.Rand.Domus;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class PersonTests
    {
        [TestInitialize]
        public void Init()
        {
            BinDirectories.Root = @"C:\Projects\31g\trunk\Code\NoFuture\Rand";
        }

        [TestMethod]
        public void AmericanTests()
        {
            DateTime? testDob = new DateTime(1974,5,6);
            var testResult = Person.American();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(Gender.Unknown, testResult.MyGender);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.LastName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.FirstName));
            Assert.IsNotNull(testResult.BirthDate);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.HomeAddress.ToString()));

            System.Diagnostics.Debug.WriteLine(testResult.NetUri.First().ToString());
            System.Diagnostics.Debug.WriteLine(testResult.WorkCity);
            System.Diagnostics.Debug.WriteLine(testResult.WorkZip);
        }

        [TestMethod]
        public void CanadianTests()
        {
            var testResult = Person.Canadian();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.HomeCity));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.HomeState));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.HomeZip));
        }

        [TestMethod]
        public void AmericanRaceTests()
        {
            const string TEST_ZIP = "92071";
            var testResult = NorthAmerican.GetAmericanRace(TEST_ZIP);
            Assert.AreNotEqual(string.Empty,testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestAmericanFemaleFirstName()
        {
            var testDob = new DateTime(1980, 10, 1);
            var testResult = NorthAmerican.GetAmericanFirstName(testDob, NoFuture.Rand.Gender.Female);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestAmericanMaleFirstName()
        {
            var testDob = new DateTime(1980, 10, 1);
            var testResult = NorthAmerican.GetAmericanFirstName(testDob, NoFuture.Rand.Gender.Male);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestAmericanLastName()
        {
            var testResult = NorthAmerican.GetAmericanLastName();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestGetAmericanMaritialStatus()
        {
            //too young 
            Assert.AreEqual(MaritialStatus.Single, NorthAmerican.GetMaritialStatus(DateTime.Today.AddYears(-16), Gender.Male));
            Assert.AreEqual(MaritialStatus.Single, NorthAmerican.GetMaritialStatus(DateTime.Today.AddYears(-16), Gender.Female));

            //cannot test further since its is random...
            System.Diagnostics.Debug.WriteLine(NorthAmerican.GetMaritialStatus(new DateTime(1974, 11, 21), Gender.Male));
            System.Diagnostics.Debug.WriteLine(NorthAmerican.GetMaritialStatus(new DateTime(1962, 1, 31), Gender.Female));
            System.Diagnostics.Debug.WriteLine(NorthAmerican.GetMaritialStatus(new DateTime(1982, 1, 31), Gender.Female));
        }

        [TestMethod]
        public void TestSolveForParent()
        {
            DateTime testDob = new DateTime(1984,4,22);
            var father = NorthAmerican.SolveForParent(testDob, NorthAmerican.LinearEquations.MaleYearOfMarriage2AvgAge, Gender.Male);
            Assert.IsNotNull(father);
            Assert.IsNotNull(father.BirthDate);
            Assert.IsTrue(father.BirthDate.Value.Year < testDob.Year);

            System.Diagnostics.Debug.WriteLine(father.FirstName);
            System.Diagnostics.Debug.WriteLine(father.LastName);
            System.Diagnostics.Debug.WriteLine(father.BirthDate);
        }

        [TestMethod]
        public void TestSolveForSpouse()
        {
            var testDob = new DateTime(1982, 8, 19);
            var spouse = NorthAmerican.SolveForSpouse(testDob, Gender.Female);
            Assert.IsNotNull(spouse);
            Assert.IsNotNull(spouse.FirstName);

            System.Diagnostics.Debug.WriteLine(spouse.FirstName);
            System.Diagnostics.Debug.WriteLine(spouse.LastName);
            System.Diagnostics.Debug.WriteLine(spouse.BirthDate);

        }

    }
}
