using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction;
using NoFuture.Rand.Law.US.Courts;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests
{
    [TestFixture]
    public class ExampleTestMinContact
    {
        [Test]
        public void TestMinimumContactIsValid00()
        {
            var testSubject = new MinimumContact(new StateCourt("NV"))
            {
                GetActiveVirtualContactLocation = lp => lp is ExampleDefendant ? new [] {new VocaBase("NV")} : null,
                GetInjuryLocation = lp => new VocaBase("NV")
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Assert.IsTrue(testResult);

            Console.WriteLine(testSubject.ToString());
        }

        [Test]
        public void TestMinimumContactIsValid01()
        {

            var testSubject = new MinimumContact(new StateCourt("NV"))
            {
                GetIntentionalTortTo = lp => lp is ExampleDefendant ? new ExamplePlaintiff() : null,
                GetDomicileLocation = lp => lp is ExamplePlaintiff ? new VocaBase("NV") : null,
            };
            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Assert.IsTrue(testResult);

            Console.WriteLine(testSubject.ToString());

        }
    }
}
