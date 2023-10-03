using System;
using NoFuture.Law.Procedure.Criminal.US;
using NoFuture.Law.Procedure.Criminal.US.SearchReasons;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleAdministrativeCauseTests
    {
        [Test]
        public void TestAdministrativeCauseIsValid00()
        {
            var testSubject = new AdministrativeCause<ExampleAdminSearchPolicy>
            {
                IsPolicyBased = t => t is ExampleAdminSearchPolicy,
                IsToDetectCriminalActivity = t => false,
                IsMinimumIntrusion = t => t is ExampleAdminSearchPolicy,
                IsSpecificDetection = t => t is ExampleAdminSearchPolicy
            };

            var testResult = testSubject.IsValid();
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class ExampleAdminSearchPolicy : Rationale
    {
    }
}
