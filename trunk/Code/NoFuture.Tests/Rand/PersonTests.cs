using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NoFuture.Rand;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Gov;
using static System.Diagnostics.Debug;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Dbg = System.Diagnostics.Debug;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class PersonTests
    {


        [TestMethod]
        public void AmericanTests()
        {
            var testDob = new DateTime(1974,5,6);
            var testResult = new NorthAmerican(testDob, Gender.Female);
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
            //Assert.IsNotNull(testResult.GetMother());
            //Assert.AreNotEqual(0, testResult.GetMother().GetChildrenAt(null));
        }

        [TestMethod]
        public void TestAmericanFull()
        {
            var testResult = new NorthAmerican(DateTime.Now.AddYears(-36), Gender.Female, true, true);
            Assert.IsNotNull(testResult.GetMother());
            Assert.AreNotEqual(0, testResult.GetMother().GetChildrenAt(null));
            Assert.IsNotNull(testResult.GetWealthAt(null));
        }

        [TestMethod]
        public void NorthAmericanEduTests()
        {
            var amer = new NorthAmerican(NAmerUtil.GetWorkingAdultBirthDate(), Gender.Female);
            var testResult = new NorthAmericanEdu(amer);

            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.HighSchool);
            WriteLine(testResult.HighSchool);
            WriteLine(testResult.College);
        }

        [TestMethod]
        public void TestGetAmericanUniversity()
        {
            var testResult = NorthAmericanEdu.GetAmericanUniversity(null);
            Assert.IsNotNull(testResult);
            testResult = NorthAmericanEdu.GetAmericanUniversity(UsState.GetStateByPostalCode("AZ"));
            Assert.IsNotNull(testResult);
        }

        [TestMethod]
        public void TestGetAmericanHighSchool()
        {
            var addrData = new AddressData()
            {
                City = "Ellisville",
                StateAbbrv = "IL",
                StateName = "Illinois",
                PostalCode = "62644"
            };

            var usHca = new UsCityStateZip(addrData);
            Assert.IsNotNull(usHca);
            var hs = NorthAmericanEdu.GetAmericanHighSchool(usHca);
            Assert.IsNotNull(hs);
            Assert.IsFalse(hs.Equals(AmericanHighSchool.GetDefaultHs()));

        }

        [TestMethod]
        public void NorthAmericanEduTestsNullPerson()
        {
            IPerson p = null;
            var testResult = new NorthAmericanEdu(p);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.HighSchool);
            WriteLine(testResult.HighSchool);
            WriteLine(testResult.College);
        }

        [TestMethod]
        public void NorthAmericanEduTestsChild()
        {
            var amer = new NorthAmerican(DateTime.Now.AddYears(-9), Gender.Female);
            var testResult = new NorthAmericanEdu(amer);
            Assert.IsNotNull(testResult);
            Assert.IsNull(testResult.HighSchool);
            Assert.IsNull(testResult.College);
        }

        [TestMethod]
        public void AmericanRaceTests()
        {
            const string TEST_ZIP = "92071";
            var testResult = NAmerUtil.GetAmericanRace(TEST_ZIP);
            Assert.AreNotEqual(string.Empty,testResult);
            WriteLine(testResult);

            testResult = NAmerUtil.GetAmericanRace("68415");
            Assert.AreEqual(NorthAmericanRace.White, testResult);
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
            var testResult = new NoFuture.Rand.Domus.NorthAmerican(DateTime.Now.AddYears(-40), Gender.Female, true, false );

            Assert.IsNotNull(testResult.GetMother());
            Assert.IsNotNull(testResult.GetFather());
            
        }

        [TestMethod]
        public void TestResolveParents()
        {
            var testResult = new NoFuture.Rand.Domus.NorthAmerican(DateTime.Now.AddYears(-40), Gender.Female);
            Assert.IsNull(testResult.GetMother());
            Assert.IsNull(testResult.GetFather());
            testResult.ResolveParents();
            Assert.IsNotNull(testResult.GetMother());
            Assert.IsNotNull(testResult.GetFather());


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
            var testSubject = new NoFuture.Rand.Domus.NorthAmerican(DateTime.Now.AddYears(-40), Gender.Female);

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
            var testPerson = new NorthAmerican(new DateTime(1955,6,20), Gender.Female, false, false);

            testPerson._children.Add(new Child(new NorthAmerican(new DateTime(1976, 10, 2), Gender.Female, false, false)));
            testPerson._children.Add(new Child(new NorthAmerican(new DateTime(1986, 3, 11), Gender.Female, false, false)));
            testPerson._children.Add(new Child(new NorthAmerican(new DateTime(1982, 12, 30), Gender.Female, false, false)));

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

            testPerson = new NorthAmerican(new DateTime(1982,4,13), Gender.Female, false, false);
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

        [TestMethod]
        public void TestGetSpouseAt()
        {
            //test with full timestamps
            var firstMarriageDate = new DateTime(DateTime.Today.Year - 15, 8, 8, DateTime.Now.Hour, DateTime.Now.Minute,
                DateTime.Now.Second);
            var firstDivorceDate = new DateTime(DateTime.Today.Year - 3, 2, 14, DateTime.Now.Hour, DateTime.Now.Minute,
                DateTime.Now.Second);
            var secondMarriageDate = new DateTime((DateTime.Today.Year - 1), 12, 23, DateTime.Now.Hour, DateTime.Now.Minute,
                DateTime.Now.Second);


            var testPerson = new NorthAmerican(new DateTime((DateTime.Today.Year - 42), 6, 20), Gender.Female);
            var firstSpouse = new NorthAmerican(new DateTime((DateTime.Today.Year - 46), 4, 4), Gender.Male);

            testPerson.AddSpouse(firstSpouse, firstMarriageDate, firstDivorceDate);

            var secondSpouse = new NorthAmerican(new DateTime((DateTime.Today.Year - 43), 12, 16), Gender.Male);
            testPerson.AddSpouse(secondSpouse, secondMarriageDate);

            //expect true when on day-of-wedding
            var testResult = testPerson.GetSpouseAt(firstMarriageDate);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Est);
            Assert.IsTrue(testResult.Est.Equals(firstSpouse));

            //expect false for day of separated
            testResult = testPerson.GetSpouseAt(firstDivorceDate);
            Assert.IsNull(testResult);

            //expect true any time between 
            for (var i = firstMarriageDate.Year + 1; i < firstDivorceDate.Year; i++)
            {
                for (var j = 1; j < 13; j++)
                {
                    testResult = testPerson.GetSpouseAt(new DateTime(i, j, 8));
                    Assert.IsNotNull(testResult);
                    Assert.IsNotNull(testResult.Est);
                    Assert.IsTrue(testResult.Est.Equals(firstSpouse));
                }
            }
            
            //expect false day directly before second marriage
            testResult = testPerson.GetSpouseAt(secondMarriageDate.Date.AddMilliseconds(-1));
            Assert.IsNull(testResult);

            testResult = testPerson.GetSpouseAt(secondMarriageDate.Date);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Est);
            Assert.IsTrue(testResult.Est.Equals(secondSpouse));
        }

        [TestMethod]
        public void TestDeathDate()
        {
            var dob = NoFuture.Rand.Domus.NAmerUtil.GetWorkingAdultBirthDate();
            var testResult = NAmerUtil.GetDeathDate(dob, Gender.Female);
            Assert.AreNotEqual(dob, testResult);
            System.Diagnostics.Debug.WriteLine("{0} - {1}", dob, testResult);

        }

        [TestMethod]
        public void TestGetNorthAmericanEdu()
        {
            var testSubject = NoFuture.Rand.Domus.Person.American();
            var testEdu = testSubject.Education;
            Assert.IsNotNull(testEdu);

            WriteLine(testEdu);

        }
    }
}
