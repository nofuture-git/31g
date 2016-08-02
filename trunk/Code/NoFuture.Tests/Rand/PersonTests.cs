using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NoFuture.Rand;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Domus;
using static System.Diagnostics.Debug;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Dbg = System.Diagnostics.Debug;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class PersonTests
    {
        [TestInitialize]
        public void Init()
        {
            BinDirectories.DataRoot = @"C:\Projects\31g\trunk\bin\Data\Source";
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
            Assert.IsNotNull(testResult.BirthCert);
            Assert.IsNotNull(testResult.Address);
            Assert.IsNotNull(testResult.Address.HomeCityArea);
            Assert.IsInstanceOfType(testResult.Address.HomeCityArea, typeof(UsCityStateZip));
            Assert.IsNotNull(((UsCityStateZip)testResult.Address.HomeCityArea).State);
            Assert.IsNotNull(testResult.DriversLicense);
            Assert.IsNotNull(testResult.GetMother());
            Assert.AreNotEqual(0, testResult.GetMother().GetChildrenAt(null));
        }

        [TestMethod]
        public void NorthAmericanEduTests()
        {
            var testContext = Person.American();
            var testResult = testContext.GetEducationAt(null);

            Assert.IsNotNull(testResult);
            WriteLine(testResult.HighSchool);
            WriteLine(testResult.College);

        }

        [TestMethod]
        public void AmericanRaceTests()
        {
            const string TEST_ZIP = "92071";
            var testResult = NAmerUtil.GetAmericanRace(TEST_ZIP);
            Assert.AreNotEqual(string.Empty,testResult);
            WriteLine(testResult);
        }

        [TestMethod]
        public void TestAmericanFemaleFirstName()
        {
            var testDob = new DateTime(1980, 10, 1);
            var testResult = NAmerUtil.GetAmericanFirstName(testDob, NoFuture.Rand.Gender.Female);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            WriteLine(testResult);
        }

        [TestMethod]
        public void TestAmericanMaleFirstName()
        {
            var testDob = new DateTime(1980, 10, 1);
            var testResult = NAmerUtil.GetAmericanFirstName(testDob, NoFuture.Rand.Gender.Male);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            WriteLine(testResult);
        }

        [TestMethod]
        public void TestAmericanLastName()
        {
            var testResult = NAmerUtil.GetAmericanLastName();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            WriteLine(testResult);
        }

        [TestMethod]
        public void TestGetAmericanMaritialStatus()
        {
            //too young 
            Assert.AreEqual(MaritialStatus.Single, NAmerUtil.GetMaritialStatus(DateTime.Today.AddYears(-16), Gender.Male));
            Assert.AreEqual(MaritialStatus.Single, NAmerUtil.GetMaritialStatus(DateTime.Today.AddYears(-16), Gender.Female));

            //cannot test further since its is random...
            WriteLine(NAmerUtil.GetMaritialStatus(new DateTime(1974, 11, 21), Gender.Male));
            WriteLine(NAmerUtil.GetMaritialStatus(new DateTime(1962, 1, 31), Gender.Female));
            WriteLine(NAmerUtil.GetMaritialStatus(new DateTime(1982, 1, 31), Gender.Female));
        }

        [TestMethod]
        public void TestSolveForParent()
        {
            DateTime testDob = new DateTime(1984,4,22);
            var father = NAmerUtil.SolveForParent(testDob, NAmerUtil.Equations.MaleAge2FirstMarriage, Gender.Male);
            Assert.IsNotNull(father);
            Assert.IsNotNull(father.BirthCert);
            Assert.IsTrue(father.BirthCert.DateOfBirth.Year < testDob.Year);

            WriteLine(father.FirstName);
            WriteLine(father.LastName);
            WriteLine(father.BirthCert.DateOfBirth);
        }

        [TestMethod]
        public void TestSolveForSpouse()
        {
            var testDob = new DateTime(1982, 8, 19);
            var spouse = NAmerUtil.SolveForSpouse(testDob, Gender.Female);
            Assert.IsNotNull(spouse);
            Assert.IsNotNull(spouse.FirstName);

            WriteLine(spouse.FirstName);
            WriteLine(spouse.LastName);
            WriteLine(spouse.BirthCert.DateOfBirth);

        }

        [TestMethod]
        public void TestFemaleDob2ProbChildless()
        {
            var testResult = NAmerUtil.Equations.FemaleYob2ProbChildless.SolveForY(1951);
            WriteLine(testResult);
            Assert.IsTrue(testResult < 0.19);
            Assert.IsTrue(testResult > 0.09);

        }

        [TestMethod]
        public void TestSolveForNumberOfChildren()
        {
            var testResult = NAmerUtil.SolveForNumberOfChildren(new DateTime(1974, 8, 11), null);

            Assert.IsTrue(-1 < testResult);

            Assert.IsTrue(5 > testResult);

            WriteLine(testResult);

            //averages, 18 year old never has children.
            for (var i = 0; i < 100; i++)
            {
                testResult = NAmerUtil.SolveForNumberOfChildren(DateTime.Now.AddYears(-18).AddDays(-128), null);
                Assert.AreEqual(0,testResult);
            }
        }

        [TestMethod]
        public void TestGetChildBirthDate()
        {
            var inputDob = DateTime.Now.AddYears(-50);

            var testResult = NAmerUtil.GetChildBirthDate(inputDob, 0, null);

            Assert.IsNotNull(testResult);

            WriteLine(testResult);

            testResult = NAmerUtil.GetChildBirthDate(inputDob, 1, null);

            Assert.IsNotNull(testResult);

            WriteLine(testResult);
        }

        [TestMethod]
        public void TestNorthAmericanWithFamily()
        {
            var testResult = new NoFuture.Rand.Domus.NorthAmerican(DateTime.Now.AddYears(-40), Gender.Female, true );

            Assert.IsNotNull(testResult.GetMother());
            Assert.IsNotNull(testResult.GetFather());

            var d = Newtonsoft.Json.JsonConvert.SerializeObject(testResult, Formatting.Indented,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            WriteLine(d);
            
        }

        [TestMethod]
        public void TestEquations()
        {
            var baseDob = new DateTime(2016,2,16,15,55,02);
            for (var i = 0; i < 15; i++)
            {
                var dob = baseDob.AddYears((25 + i) * -1);
                var marriageEq = NAmerUtil.Equations.FemaleAge2FirstMarriage.SolveForY(dob.ToDouble());
                var firstBornEq = NAmerUtil.Equations.FemaleAge2FirstChild.SolveForY(dob.Year);

                WriteLine($"dob: {dob};  marriageAge: {marriageEq}; firstBornAge: {firstBornEq};");

            }
        }

        [TestMethod]
        public void TestGetMaritalStatus()
        {
            var testSubject = new NoFuture.Rand.Domus.NorthAmerican(DateTime.Now.AddYears(-40), Gender.Female, true);

            var testResult = testSubject.GetMaritalStatusAt(null);

            Assert.AreNotEqual(MaritialStatus.Unknown, testResult);

            WriteLine(testResult);

            testResult = testSubject.GetMaritalStatusAt(DateTime.Now.AddYears(-10));

            Assert.AreNotEqual(MaritialStatus.Unknown, testResult);

            WriteLine(testResult);
        }

        [TestMethod]
        public void TestIsValidDobOfChild()
        {
            var testPerson = new NorthAmerican(new DateTime(1955,6,20), Gender.Female, false);

            testPerson._children.Add(new Child(new NorthAmerican(new DateTime(1976, 10, 2), Gender.Female, false)));
            testPerson._children.Add(new Child(new NorthAmerican(new DateTime(1986, 3, 11), Gender.Female, false)));
            testPerson._children.Add(new Child(new NorthAmerican(new DateTime(1982, 12, 30), Gender.Female, false)));

            var testDob = new DateTime(1985, 9, 10);//conception ~ 12/4/1984

            var testResult = testPerson.IsValidDobOfChild(testDob);

            //invalid: dob during prev preg
            Assert.IsFalse(testResult);

            testDob = testDob.AddDays(313);//conception ~ 10/13/1985, dob ~ 7/20/1986

            //invalid: conception during prev preg
            testResult = testPerson.IsValidDobOfChild(testDob);
            Assert.IsFalse(testResult);

            testDob = testDob.AddDays(313);//conception ~ 8/22/1986, dob 5/29/1987 

            //valid: conception ~ 5 months after prev birth
            testResult = testPerson.IsValidDobOfChild(testDob);
            Assert.IsTrue(testResult);

            testPerson = new NorthAmerican(new DateTime(1982,4,13), Gender.Female, false );
            testPerson._children.Add(new Child(new NorthAmerican(new DateTime(2007, 8, 30), Gender.Male)));
            testPerson._children.Add(new Child(new NorthAmerican(new DateTime(2009, 12, 20), Gender.Female)));

            testDob = new DateTime(2009,3,6);
            Assert.IsFalse(testPerson.IsValidDobOfChild(testDob));
            testDob = testDob.AddDays(280 + 28);
            Assert.IsFalse(testPerson.IsValidDobOfChild(testDob));
            testDob = testDob.AddDays(280 + 28);
            
            testResult = testPerson.IsValidDobOfChild(testDob);
            Assert.IsTrue(testResult);
            WriteLine(testDob);
        }
    }
}
