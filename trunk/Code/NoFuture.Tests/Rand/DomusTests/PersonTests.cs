using System;
using System.Linq;
using NUnit.Framework;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Domus.US;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Tele;

namespace NoFuture.Rand.Tests.DomusTests
{
    [TestFixture]
    public class PersonTests
    {
        [Test]
        public void AmericanTests()
        {
            var testDob = new DateTime(1974,5,6);
            var testResult = American.RandomAmerican(testDob, Gender.Female);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(Gender.Unknown, testResult.Gender);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.LastName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.FirstName));
            Assert.IsNotNull(testResult.BirthCert);
        }

        [Test]
        public void TestAmericanFull()
        {
            var testResult = American.RandomAmerican(DateTime.UtcNow.AddYears(-36), Gender.Female, true);
            Assert.IsNotNull(testResult.GetBiologicalMother());
            Assert.AreNotEqual(0, testResult.GetBiologicalMother().GetChildrenAt(null));
        }

        [Test]
        public void NorthAmericanEduTests()
        {
            var amer = American.RandomAmerican(Etx.RandomAdultBirthDate(), Gender.Female);
            var testResult = amer.GetRandomEducation();

            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.HighSchool);
            Console.WriteLine(testResult.HighSchool);
            Console.WriteLine(testResult.College);
        }

        [Test]
        public void TestAmericanFemaleFirstName()
        {
            var testDob = new DateTime(1980, 10, 1);
            var testResult = AmericanUtil.RandomAmericanFirstName(Gender.Female, testDob);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestAmericanMaleFirstName()
        {
            var testDob = new DateTime(1980, 10, 1);
            var testResult = AmericanUtil.RandomAmericanFirstName(Gender.Male, testDob);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestAmericanLastName()
        {
            var testResult = AmericanUtil.RandomAmericanLastName();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestGetAmericanMaritialStatus()
        {
            //too young 
            Assert.AreEqual(MaritalStatus.Single, AmericanData.RandomMaritialStatus(DateTime.Today.AddYears(-16), Gender.Male));
            Assert.AreEqual(MaritalStatus.Single, AmericanData.RandomMaritialStatus(DateTime.Today.AddYears(-16), Gender.Female));

            //cannot test further since its is random...
            Console.WriteLine(AmericanData.RandomMaritialStatus(new DateTime(1974, 11, 21), Gender.Male));
            Console.WriteLine(AmericanData.RandomMaritialStatus(new DateTime(1962, 1, 31), Gender.Female));
            Console.WriteLine(AmericanData.RandomMaritialStatus(new DateTime(1982, 1, 31), Gender.Female));
        }

        [Test]
        public void TestSolveForParent()
        {
            DateTime testDob = new DateTime(1984,4,22);
            var father = AmericanUtil.RandomParent(testDob, Gender.Male);
            Assert.IsNotNull(father);
            Assert.IsNotNull(father.BirthCert);
            Assert.IsTrue(father.BirthCert.DateOfBirth.Year < testDob.Year);

            Console.WriteLine(father.FirstName);
            Console.WriteLine(father.LastName);
            Console.WriteLine(father.BirthCert.DateOfBirth);
        }

        [Test]
        public void TestRandomParentBirthDate()
        {
            DateTime testDob = new DateTime(1984, 4, 22);
            var parentDob = AmericanUtil.RandomParentBirthDate(testDob);
            var yearsExpected = 365 * 14;

            var diff = testDob - parentDob;
            Assert.IsTrue(diff.TotalDays > yearsExpected);
        }

        [Test]
        public void TestSolveForSpouse()
        {
            var testDob = new DateTime(1982, 8, 19);
            var spouse = AmericanUtil.RandomSpouse(testDob, Gender.Female);
            Assert.IsNotNull(spouse);
            Assert.IsNotNull(spouse.FirstName);

            Console.WriteLine(spouse.FirstName);
            Console.WriteLine(spouse.LastName);
            Console.WriteLine(spouse.BirthCert.DateOfBirth);

        }

        [Test]
        public void TestFemaleDob2ProbChildless()
        {
            var testResult = AmericanEquations.FemaleYob2ProbChildless.SolveForY(1951);
            Console.WriteLine(testResult);
            Assert.IsTrue(testResult < 0.19);
            Assert.IsTrue(testResult > 0.0001);

        }

        [Test]
        public void TestSolveForNumberOfChildren()
        {
            var testResult = AmericanUtil.RandomNumberOfChildren(new DateTime(1974, 8, 11), null);

            Assert.IsTrue(-1 < testResult);

            Assert.IsTrue(5 > testResult);

            Console.WriteLine(testResult);

            //averages, 18 year old never has children.
            for (var i = 0; i < 100; i++)
            {
                testResult = AmericanUtil.RandomNumberOfChildren(DateTime.UtcNow.AddYears(-18).AddDays(-128), null);
                Assert.AreEqual(0,testResult);
            }
        }

        [Test]
        public void TestGetChildBirthDate()
        {
            var inputDob = DateTime.UtcNow.AddYears(-50);

            var testResult = AmericanUtil.RandomChildBirthDate(inputDob, 0, null);

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);

            testResult = AmericanUtil.RandomChildBirthDate(inputDob, 1, null);

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);
        }

        [Test]
        public void TestNorthAmericanWithFamily()
        {
            var testResult = American.RandomAmerican(DateTime.UtcNow.AddYears(-40), Gender.Female, true);

            Assert.IsNotNull(testResult.GetBiologicalMother());
            Assert.IsNotNull(testResult.GetBiologicalFather());
            
        }

        [Test]
        public void TestResolveParents()
        {
            var testResult = American.RandomAmerican(DateTime.UtcNow.AddYears(-40), Gender.Female);
            Assert.IsNull(testResult.GetBiologicalMother());
            Assert.IsNull(testResult.GetBiologicalFather());
            testResult.ResolveParents();
            Assert.IsNotNull(testResult.GetBiologicalMother());
            Assert.IsNotNull(testResult.GetBiologicalFather());


        }

        [Test]
        public void TestGetMaritalStatus()
        {
            var testSubject = American.RandomAmerican(DateTime.UtcNow.AddYears(-40), Gender.Female);

            var testResult = testSubject.GetMaritalStatusAt(null);

            Assert.AreNotEqual(MaritalStatus.Unknown, testResult);

            Console.WriteLine(testResult);

            testResult = testSubject.GetMaritalStatusAt(DateTime.UtcNow.AddYears(-10));

            Assert.AreNotEqual(MaritalStatus.Unknown, testResult);

            Console.WriteLine(testResult);
        }

        [Test]
        public void TestIsValidDobOfChild()
        {
            var testPerson = American.RandomAmerican(new DateTime(1955,6,20), Gender.Female, false);

            testPerson.AddChild(American.RandomAmerican(new DateTime(1976, 10, 2), Gender.Female, false));
            testPerson.AddChild(American.RandomAmerican(new DateTime(1986, 3, 11), Gender.Female, false));
            testPerson.AddChild(American.RandomAmerican(new DateTime(1982, 12, 30), Gender.Female, false));

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

            testPerson = American.RandomAmerican(new DateTime(1982,4,13), Gender.Female, false);
            testPerson.AddChild(American.RandomAmerican(new DateTime(2007, 8, 30), Gender.Male));
            testPerson.AddChild(American.RandomAmerican(new DateTime(2009, 12, 20), Gender.Female));

            testDob = new DateTime(2009,3,6);
            Assert.IsFalse(testPerson.IsValidDobOfChild(testDob));
            testDob = testDob.AddDays(280 + 28);
            Assert.IsFalse(testPerson.IsValidDobOfChild(testDob));
            testDob = testDob.AddDays(280 + 28);
            
            testResult = testPerson.IsValidDobOfChild(testDob);
            Assert.IsTrue(testResult);
            Console.WriteLine(testDob);
        }

        [Test]
        public void TestGetSpouseAt()
        {
            //test with full timestamps
            var firstMarriageDate = new DateTime(DateTime.Today.Year - 15, 8, 8, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute,
                DateTime.UtcNow.Second);
            var firstDivorceDate = new DateTime(DateTime.Today.Year - 3, 2, 14, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute,
                DateTime.UtcNow.Second);
            var secondMarriageDate = new DateTime((DateTime.Today.Year - 1), 12, 23, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute,
                DateTime.UtcNow.Second);


            var testPerson = American.RandomAmerican(new DateTime((DateTime.Today.Year - 42), 6, 20), Gender.Female);
            var firstSpouse = American.RandomAmerican(new DateTime((DateTime.Today.Year - 46), 4, 4), Gender.Male);

            testPerson.AddSpouse(firstSpouse, firstMarriageDate, firstDivorceDate);

            var secondSpouse = American.RandomAmerican(new DateTime((DateTime.Today.Year - 43), 12, 16), Gender.Male);
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

        [Test]
        public void TestDeathDate()
        {
            var dob = Etx.RandomAdultBirthDate();
            var testResult = AmericanDeathCert.RandomDeathDate(dob, Gender.Female.ToString());
            Assert.AreNotEqual(dob, testResult);
            Console.WriteLine("{0} - {1}", dob, testResult);

        }

        [Test]
        public void TestGetNorthAmericanEdu()
        {
            var testSubject = American.RandomAmerican();
            var testEdu = testSubject.GetEducationAt(null);
            Assert.IsNotNull(testEdu);

            Console.WriteLine(testEdu);
        }

        [Test]
        public void TestGetParent()
        {
            var testSubject = American.RandomAmerican();
            Assert.IsNotNull(testSubject.Parents);
            Assert.AreNotEqual(0,testSubject.Parents.Count());

        }

        [Test]
        public void TestAddParent()
        {
            var testMother = American.RandomAmerican(new DateTime(1970, 1, 1), Gender.Female);
            var testChild =
                American.RandomAmerican(testMother.BirthCert.DateOfBirth.AddYears(25).AddDays(28), Gender.Female);

            testChild.AddParent(testMother, KindsOfNames.Biological | KindsOfNames.Mother);

            Assert.AreEqual(1, testChild.Parents.Count());

            var testResult =
                testChild.Parents.FirstOrDefault(c => c.AnyNames(k => k == (KindsOfNames.Biological | KindsOfNames.Mother)));
            Assert.IsNotNull(testResult);
            Assert.AreEqual(testMother, testResult.Est);

            //nothing changes 
            testChild.AddParent(testMother, KindsOfNames.Biological | KindsOfNames.Mother);
            testResult =
                testChild.Parents.FirstOrDefault(c => c.AnyNames(k => k == (KindsOfNames.Biological | KindsOfNames.Mother)));
            Assert.IsNotNull(testResult);
            Assert.AreEqual(testMother, testResult.Est);

            //adding a Parent does not add a child 
            Assert.AreEqual(0, testResult.Est.Children.Count());

            //cannot add when biological limits are exceeded
            testMother = American.RandomAmerican(new DateTime(1993, 1, 1), Gender.Female);
            testChild =
                American.RandomAmerican(new DateTime(1996, 1, 1), Gender.Female);

            testChild.AddParent(testMother, KindsOfNames.Mother | KindsOfNames.Biological);
            Assert.AreEqual(0, testChild.Parents.Count());
        }

        [Test]
        public void TestAddChild()
        {
            var testMother = American.RandomAmerican(new DateTime(1970, 1, 1), Gender.Female);
            var testChild00 =
                American.RandomAmerican(testMother.BirthCert.DateOfBirth.AddYears(25).AddDays(28), Gender.Female);

            testMother.AddChild(testChild00);

            Assert.AreEqual(1, testMother.Children.Count());

            var testChild01 = American.RandomAmerican(testChild00.BirthCert.DateOfBirth.AddDays(28), Gender.Female);

            //cannot add this child since DoB violates biological constraints
            testMother.AddChild(testChild01);
            Assert.AreEqual(1, testMother.Children.Count());

            //but this is ok
            testMother.AddChild(testChild01, KindsOfNames.Mother | KindsOfNames.Adopted);
            Assert.AreEqual(2, testMother.Children.Count());

            //adding child does not add parent
            Assert.IsNull(testChild01.GetParent(KindsOfNames.Mother | KindsOfNames.Adopted));
        }

        [Test]
        public void TestAsPoco()
        {
            var dob = Etx.RandomAdultBirthDate();
            var gender = Gender.Female;
            var firstName = AmericanUtil.RandomAmericanFirstName(gender, dob);
            var middleName = AmericanUtil.RandomAmericanFirstName(gender, dob);
            var lastName = AmericanUtil.RandomAmericanLastName();

            var testSubject = new American
            {
                BirthCert = new AmericanBirthCert(dob),
                Gender = gender,
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName
            };

            var addrLine1 = "4455 Deweier St.";
            var addrLine2 = "Huntington Beach, CA 90802";

            PostalAddress.TryParse(addrLine1, addrLine2, out var address);
            testSubject.AddAddress(address);

            Assert.AreEqual($"{firstName} {middleName} {lastName}", testSubject.FullName);
            Assert.IsNotNull(testSubject.Address);
            Assert.AreEqual($"{addrLine1}\r\n{addrLine2}", testSubject.Address.ToString());

            var phNum = "707-884-5563";
            NorthAmericanPhone.TryParse(phNum, out var naph);
            naph.Descriptor = KindsOfLabels.Mobile;
            testSubject.AddPhone(naph);
    
            var testResultPhone = testSubject.PhoneNumbers.FirstOrDefault(p => p.Descriptor == KindsOfLabels.Mobile);
            Assert.IsNotNull(testResultPhone as NorthAmericanPhone);
            var namerTestResultPhone = testResultPhone as NorthAmericanPhone;
            Assert.AreEqual("707", namerTestResultPhone.AreaCode);
            Assert.AreEqual("884", namerTestResultPhone.CentralOfficeCode);
            Assert.AreEqual("5563", namerTestResultPhone.SubscriberNumber);

            var testEmail = "Gauis.Ceaser@romanempire.net";
            testSubject.AddUri(testEmail);

            var testResultEmail = testSubject.NetUris.FirstOrDefault(u => u.Scheme == Uri.UriSchemeMailto);
            Assert.IsNotNull(testResultEmail);
            Assert.IsTrue(testResultEmail.ToString().EndsWith(testEmail));
        }

        [Test]
        public void TestToData()
        {
            var testSubject = American.RandomAmerican(DateTime.UtcNow.AddYears(-36), Gender.Female, true);
            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach(var tr in testResult.Keys)
                Console.WriteLine($"{tr}, {testResult[tr]}");
        }
    }
}
