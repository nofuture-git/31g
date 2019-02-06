using System;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Criminal;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.CriminalTests
{
    [TestFixture]
    public class RobinsonvCaliforniaTests
    {
        [Test]
        public void RobinsonvCalifornia()
        {
            var testSubject = new Felony();
            testSubject.Concurrence.ActusReus.IsStatus = lp => (lp as Robinson)?.IsDrugAddict ?? false;
            testSubject.Concurrence.ActusReus.IsVoluntary = lp => true;

            var testResult = testSubject.IsValid(new Robinson());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Robinson : LegalPerson
    {
        public Robinson():base("ROBINSON") { }
        public bool IsDrugAddict => true;
    }

    public class California : Government
    {

    }
}
