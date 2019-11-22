using System;
using NoFuture.Rand.Law.Procedure.Civil.US.Pleadings;
using NoFuture.Rand.Law.Procedure.Civil.US.ServiceOfProcess;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Courts;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests.PleadingsTests
{
    [TestFixture]
    public class ExampleTestAmendment
    {
        [Test]
        public void TestAmendmentIsValid()
        {
            var testSubject = new Amendment()
            {
                Court = new StateCourt("CA"),
                GetServiceOfProcess = lp => new VoluntaryEntry { GetToDateOfService = lp1 => DateTime.UtcNow.AddDays(-14) },
                LinkedTo = new Complaint(),
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);


            testSubject = new Amendment()
            {
                Court = new StateCourt("CA"),
                GetServiceOfProcess = lp => new VoluntaryEntry { GetToDateOfService = lp1 => DateTime.UtcNow.AddDays(-45) },
                LinkedTo = new Complaint(),
            };

            testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestAmendmentIsValidWithLeave()
        {
            var testSubject = new Amendment()
            {
                Court = new StateCourt("CA"),
                GetServiceOfProcess = lp => new VoluntaryEntry { GetToDateOfService = lp1 => DateTime.UtcNow.AddDays(-45) },
                LinkedTo = new Complaint(),
                //since its linked to a complaint (and not an answer), the opposition is the defense
                Assent =  new Consent { IsApprovalExpressed = lp => lp is IDefendant}
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }
}
