using System;
using NoFuture.Rand.Law.Procedure.Criminal.US.Intrusions;
using NoFuture.Rand.Law.Procedure.Criminal.US.SearchReasons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleExigentCircumstancesTests
    {
        [Test]
        public void TestExigentCircumstancesIsValid00()
        {
            var testSubject = new Arrest
            {
                IsAwareOfBeingArrested = lp => true,
                ProbableCause = new ExampleExigentCircumstances(),
                IsOccurInPublicPlace = lp => false,
            };

            var testResult = testSubject.IsValid(new ExampleSuspect(), new ExampleLawEnforcement());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class ExampleExigentCircumstances : ExigentCircumstances
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
