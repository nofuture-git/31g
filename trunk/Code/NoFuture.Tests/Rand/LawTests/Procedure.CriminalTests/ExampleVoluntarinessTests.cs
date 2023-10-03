using System;
using NoFuture.Law.Procedure.Criminal.US.Interrogations;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleVoluntarinessTests
    {
        [Test]
        public void TestVoluntarinessIsValid00()
        {
            var testSubject = new Voluntariness
            {
                IsSubjectedToCoercivePoliceConduct = lp => lp is ExampleSuspect,
                IsSufficientToOvercomeWillpower = lp => lp is ExampleSuspect
            };

            var testResult = testSubject.IsValid(new ExampleLawEnforcement(), new ExampleSuspect());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }
}
