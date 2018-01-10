using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Domus.US;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Gov;
using NoFuture.Util.Core;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NoFuture.Rand.Tests
{
    [TestClass]
    public class PersonTests
    {


        [TestMethod]
        public void AmericanTests()
        {
            var testDob = new DateTime(1974,5,6);
            var testResult = new American(testDob, Gender.Female);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(Gender.Unknown, testResult.MyGender);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.LastName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.FirstName));
            Assert.IsNotNull(testResult.BirthCert);
        }

        [TestMethod]
        public void TestAmericanFull()
        {
            var testResult = new American(DateTime.Now.AddYears(-36), Gender.Female, true);
            Assert.IsNotNull(testResult.GetMother());
            Assert.AreNotEqual(0, testResult.GetMother().GetChildrenAt(null));
        }

        [TestMethod]
        public void NorthAmericanEduTests()
        {
            var amer = new American(UsState.GetWorkingAdultBirthDate(), Gender.Female);
            var testResult = amer.GetEducationByPerson();

            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.HighSchool);
            Debug.WriteLine(testResult.HighSchool);
            Debug.WriteLine(testResult.College);
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
            DateTime? dob = null;
            UsCityStateZip homeCityArea = null;
            var testResult = new NorthAmericanEdu(dob, homeCityArea);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.HighSchool);
            Assert.IsNotNull(testResult.HighSchool.Item1);
            Debug.WriteLine(testResult.HighSchool);
            Debug.WriteLine(testResult.College);
        }

        [TestMethod]
        public void NorthAmericanEduTestsChild()
        {
            var amer = new American(DateTime.Now.AddYears(-9), Gender.Female);
            var testResult = amer.GetEducationByPerson();
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.HighSchool);
            Assert.IsNull(testResult.HighSchool.Item1);
            Assert.IsNotNull(testResult.College);
            Assert.IsNull(testResult.College.Item1);
        }

        [TestMethod]
        public void AmericanRaceTests()
        {
            const string TEST_ZIP = "92071";
            var testResult = AmericanUtil.GetAmericanRace(TEST_ZIP);
            Assert.AreNotEqual(string.Empty,testResult);
            Debug.WriteLine(testResult);

            testResult = AmericanUtil.GetAmericanRace("68415");
            Assert.AreEqual(NorthAmericanRace.White, testResult);
        }

        [TestMethod]
        public void TestAmericanFemaleFirstName()
        {
            var testDob = new DateTime(1980, 10, 1);
            var testResult = AmericanUtil.GetAmericanFirstName(testDob, Gender.Female);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestAmericanMaleFirstName()
        {
            var testDob = new DateTime(1980, 10, 1);
            var testResult = AmericanUtil.GetAmericanFirstName(testDob, Gender.Male);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestAmericanLastName()
        {
            var testResult = AmericanUtil.GetAmericanLastName();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestGetAmericanMaritialStatus()
        {
            //too young 
            Assert.AreEqual(MaritialStatus.Single, AmericanUtil.GetMaritialStatus(DateTime.Today.AddYears(-16), Gender.Male));
            Assert.AreEqual(MaritialStatus.Single, AmericanUtil.GetMaritialStatus(DateTime.Today.AddYears(-16), Gender.Female));

            //cannot test further since its is random...
            Debug.WriteLine(AmericanUtil.GetMaritialStatus(new DateTime(1974, 11, 21), Gender.Male));
            Debug.WriteLine(AmericanUtil.GetMaritialStatus(new DateTime(1962, 1, 31), Gender.Female));
            Debug.WriteLine(AmericanUtil.GetMaritialStatus(new DateTime(1982, 1, 31), Gender.Female));
        }

        [TestMethod]
        public void TestSolveForParent()
        {
            DateTime testDob = new DateTime(1984,4,22);
            var father = AmericanUtil.SolveForParent(testDob, AmericanEquations.MaleAge2FirstMarriage, Gender.Male);
            Assert.IsNotNull(father);
            Assert.IsNotNull(father.BirthCert);
            Assert.IsTrue(father.BirthCert.DateOfBirth.Year < testDob.Year);

            Debug.WriteLine(father.FirstName);
            Debug.WriteLine(father.LastName);
            Debug.WriteLine(father.BirthCert.DateOfBirth);
        }

        [TestMethod]
        public void TestSolveForSpouse()
        {
            var testDob = new DateTime(1982, 8, 19);
            var spouse = AmericanUtil.SolveForSpouse(testDob, Gender.Female);
            Assert.IsNotNull(spouse);
            Assert.IsNotNull(spouse.FirstName);

            Debug.WriteLine(spouse.FirstName);
            Debug.WriteLine(spouse.LastName);
            Debug.WriteLine(spouse.BirthCert.DateOfBirth);

        }

        [TestMethod]
        public void TestFemaleDob2ProbChildless()
        {
            var testResult = AmericanEquations.FemaleYob2ProbChildless.SolveForY(1951);
            Debug.WriteLine(testResult);
            Assert.IsTrue(testResult < 0.19);
            Assert.IsTrue(testResult > 0.09);

        }

        [TestMethod]
        public void TestSolveForNumberOfChildren()
        {
            var testResult = AmericanUtil.SolveForNumberOfChildren(new DateTime(1974, 8, 11), null);

            Assert.IsTrue(-1 < testResult);

            Assert.IsTrue(5 > testResult);

            Debug.WriteLine(testResult);

            //averages, 18 year old never has children.
            for (var i = 0; i < 100; i++)
            {
                testResult = AmericanUtil.SolveForNumberOfChildren(DateTime.Now.AddYears(-18).AddDays(-128), null);
                Assert.AreEqual(0,testResult);
            }
        }

        [TestMethod]
        public void TestGetChildBirthDate()
        {
            var inputDob = DateTime.Now.AddYears(-50);

            var testResult = AmericanUtil.GetChildBirthDate(inputDob, 0, null);

            Assert.IsNotNull(testResult);

            Debug.WriteLine(testResult);

            testResult = AmericanUtil.GetChildBirthDate(inputDob, 1, null);

            Assert.IsNotNull(testResult);

            Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestNorthAmericanWithFamily()
        {
            var testResult = new American(DateTime.Now.AddYears(-40), Gender.Female, true);

            Assert.IsNotNull(testResult.GetMother());
            Assert.IsNotNull(testResult.GetFather());
            
        }

        [TestMethod]
        public void TestResolveParents()
        {
            var testResult = new American(DateTime.Now.AddYears(-40), Gender.Female);
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
                var marriageEq = AmericanEquations.FemaleAge2FirstMarriage.SolveForY(dob.ToDouble());
                var firstBornEq = AmericanEquations.FemaleAge2FirstChild.SolveForY(dob.Year);

                Debug.WriteLine($"dob: {dob};  marriageAge: {marriageEq}; firstBornAge: {firstBornEq};");

            }
        }

        [TestMethod]
        public void TestGetMaritalStatus()
        {
            var testSubject = new American(DateTime.Now.AddYears(-40), Gender.Female);

            var testResult = testSubject.GetMaritalStatusAt(null);

            Assert.AreNotEqual(MaritialStatus.Unknown, testResult);

            Debug.WriteLine(testResult);

            testResult = testSubject.GetMaritalStatusAt(DateTime.Now.AddYears(-10));

            Assert.AreNotEqual(MaritialStatus.Unknown, testResult);

            Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestIsValidDobOfChild()
        {
            var testPerson = new American(new DateTime(1955,6,20), Gender.Female, false);

            testPerson._children.Add(new Child(new American(new DateTime(1976, 10, 2), Gender.Female, false)));
            testPerson._children.Add(new Child(new American(new DateTime(1986, 3, 11), Gender.Female, false)));
            testPerson._children.Add(new Child(new American(new DateTime(1982, 12, 30), Gender.Female, false)));

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

            testPerson = new American(new DateTime(1982,4,13), Gender.Female, false);
            testPerson._children.Add(new Child(new American(new DateTime(2007, 8, 30), Gender.Male)));
            testPerson._children.Add(new Child(new American(new DateTime(2009, 12, 20), Gender.Female)));

            testDob = new DateTime(2009,3,6);
            Assert.IsFalse(testPerson.IsValidDobOfChild(testDob));
            testDob = testDob.AddDays(280 + 28);
            Assert.IsFalse(testPerson.IsValidDobOfChild(testDob));
            testDob = testDob.AddDays(280 + 28);
            
            testResult = testPerson.IsValidDobOfChild(testDob);
            Assert.IsTrue(testResult);
            Debug.WriteLine(testDob);
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


            var testPerson = new American(new DateTime((DateTime.Today.Year - 42), 6, 20), Gender.Female);
            var firstSpouse = new American(new DateTime((DateTime.Today.Year - 46), 4, 4), Gender.Male);

            testPerson.AddSpouse(firstSpouse, firstMarriageDate, firstDivorceDate);

            var secondSpouse = new American(new DateTime((DateTime.Today.Year - 43), 12, 16), Gender.Male);
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
            var dob = UsState.GetWorkingAdultBirthDate();
            var testResult = AmericanUtil.GetDeathDate(dob, Gender.Female);
            Assert.AreNotEqual(dob, testResult);
            Debug.WriteLine("{0} - {1}", dob, testResult);

        }

        [TestMethod]
        public void TestGetNorthAmericanEdu()
        {
            var testSubject = Person.American();
            var testEdu = testSubject.Education;
            Assert.IsNotNull(testEdu);

            Debug.WriteLine(testEdu);

        }
    }
}
