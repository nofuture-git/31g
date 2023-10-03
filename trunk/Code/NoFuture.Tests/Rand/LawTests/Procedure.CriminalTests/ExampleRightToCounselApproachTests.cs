using System;
using System.Linq;
using NoFuture.Law.Procedure.Criminal.US.Interrogations;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleRightToCounselApproachTests
    {
        [Test]
        public void TestRightToCounselApproachIsValid00()
        {
            var testSubject = new RightToCounselApproach
            {
                IsDeliberateElicit = lp => lp is ExampleLawEnforcement,
                CurrentDateTime =  DateTime.UtcNow.AddDays(2),
                GetDateInitiationJudicialProceedings = lps => lps.Any(lp => lp is ExampleSuspect) ? new DateTime?(DateTime.UtcNow.AddDays(-2)) : null,
            };

            var testResult = testSubject.IsValid(new ExampleSuspect(), new ExampleLawEnforcement());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }
}
