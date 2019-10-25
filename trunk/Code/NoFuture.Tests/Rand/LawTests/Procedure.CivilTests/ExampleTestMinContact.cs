using System;
using NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests
{
    [TestFixture]
    public class ExampleTestMinContact
    {
        [Test]
        public void TestMinimumContactIsValid()
        {
            var testSubject = new MinimumContact()
            {
                IsActiveVirtualContact = lp => lp is ExampleDefendant
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Assert.IsTrue(testResult);

            Console.WriteLine(testSubject.ToString());
        }
    }
}
