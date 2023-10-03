using System;
using NoFuture.Law.Procedure.Criminal.US.Witness;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleEyewitnessIdentificationTests
    {
        [Test]
        public void TestEyewitnessIdentificationIsValid00()
        {
            var testSubject = new EyewitnessIdentification
            {
                IsProcedureSuggestive = lp => lp is ExampleSuspect,
                IsProcedureUnnecessary = lp => lp is ExampleSuspect,
                IsProcedureUnreliable = lp => lp is ExampleSuspect
            };

            var testResult = testSubject.IsValid(new ExampleLawEnforcement(), new ExampleSuspect());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);

        }
    }
}
