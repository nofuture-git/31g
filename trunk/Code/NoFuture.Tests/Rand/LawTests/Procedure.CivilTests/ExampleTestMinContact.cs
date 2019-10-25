using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests
{
    [TestFixture]
    public class ExampleTestMinContact
    {
        [Test]
        public void TestMinimumContactIsValid00()
        {
            var testSubject = new MinimumContact("Spook City NV")
            {
                GetActiveVirtualContactLocation = lp => lp is ExampleDefendant ? new [] {new VocaBase("Spook City NV")} : null,
                GetInjuryLocation = lp => new VocaBase("Spook City NV")
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Assert.IsTrue(testResult);

            Console.WriteLine(testSubject.ToString());
        }

        [Test]
        public void TestMinimumContactIsValid01()
        {

            var testSubject = new MinimumContact("Spook City NV")
            {
                GetIntentionalTortTo = lp => lp is ExampleDefendant ? new ExamplePlaintiff() : null,
                GetDomicileLocation = lp => lp is ExamplePlaintiff ? new VocaBase("Spook City NV") : null,
            };
            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Assert.IsTrue(testResult);

            Console.WriteLine(testSubject.ToString());

        }
    }
}
