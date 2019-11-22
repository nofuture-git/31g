using System;
using NoFuture.Rand.Law.Procedure.Civil.US.ServiceOfProcess;
using NoFuture.Rand.Law.US.Courts;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests.ServiceOfProcessTests
{
    [TestFixture]
    public class ExampleTestVoluntaryEntry
    {
        [Test]
        public void TestVoluntaryEntryIsValid()
        {
            var testSubject = new VoluntaryEntry
            {
                Court = new StateCourt("WV"),
                IsSigned = lp => lp is IDefendant || lp is INotaryPublic,
                GetToDateOfService = lp => DateTime.Today.AddDays(-14),
            };

            var testResult =
                testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant(), new ExampleNotaryPublic());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class ExampleNotaryPublic : LegalPerson, INotaryPublic
    {
        public ExampleNotaryPublic() : base("Notia P. Ublic") { }
    }
}
