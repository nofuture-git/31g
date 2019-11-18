using System;
using NoFuture.Rand.Law.Procedure.Civil.US.ServiceOfProcess;
using NoFuture.Rand.Law.US.Courts;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests.ServiceOfProcessTests
{
    [TestFixture]
    public class ExampleTestInPersonDelivery
    {
        [Test]
        public void TestInPersonDeliveryIsValid()
        {
            var testSubject = new InPersonDelivery
            {
                Court = new StateCourt("UT"),
                GetDeliveredTo = lp => lp is ICourtOfficial ? new ExampleDefendant() : null,
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant(),
                new ExampleLawEnforcement());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class ExampleLawEnforcement : LegalPerson, ILawEnforcement
    {
        public ExampleLawEnforcement() :base("Johnny Law") { }
    }
}
