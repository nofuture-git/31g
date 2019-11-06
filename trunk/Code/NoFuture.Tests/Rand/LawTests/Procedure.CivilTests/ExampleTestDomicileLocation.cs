using System;
using System.Linq;
using NoFuture.Rand.Law.Procedure.Civil.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests
{
    [TestFixture]
    public class ExampleTestDomicileLocation
    {
        [Test]
        public void TestDomicileLocationIsValid()
        {
            var testSubject = new DomicileLocation("Ohio")
            {
                IsIntendsIndefiniteStay = lp => lp is ExamplePlaintiff,
                IsPhysicallyPresent = lp => lp is ExamplePlaintiff,
                GetSubjectPerson = lps => lps.FirstOrDefault(l => l is ExamplePlaintiff)
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff());
            Assert.IsTrue(testResult);

            testResult = testSubject.IsValid(new ExampleDefendant());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }
}
