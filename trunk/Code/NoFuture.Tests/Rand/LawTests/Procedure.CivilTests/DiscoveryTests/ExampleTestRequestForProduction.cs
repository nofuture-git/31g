using System;
using NoFuture.Rand.Law.Procedure.Civil.US.Discovery;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Courts;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests.DiscoveryTests
{
    [TestFixture]
    public class ExampleTestRequestForProduction
    {
        [Test]
        public void TestRequestForProductionIsValid00()
        {
            var testSubject = new RequestForProduction
            {
                Court = new StateCourt("AL"),
                GetSubjectPerson = lps => lps.Plaintiff(),
                GetAssertion = lp => lp is IPlaintiff ? new ExampleCauseForAction() : null,
                IsLimitedByCourtOrder = lc => lc is ExampleCauseForAction
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);

        }

        [Test]
        public void TestRequestForProductionIsValid01()
        {
            var testSubject = new RequestForProduction
            {
                Court = new StateCourt("AL"),
                GetSubjectPerson = lps => lps.Plaintiff(),
                GetAssertion = lp => lp is IPlaintiff ? new ExampleCauseForAction() : null,
                IsIrrelevantToPartyClaimOrDefense = (lp, lc) => lc is ExampleCauseForAction
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);

        }

        [Test]
        public void TestRequestForProductionIsValid02()
        {
            var testSubject = new RequestForProduction
            {
                Court = new StateCourt("AL"),
                GetSubjectPerson = lps => lps.Plaintiff(),
                GetAssertion = lp => lp is IPlaintiff ? new ExampleCauseForAction() : null,
                IsPrivilegedMatter = lc => lc is ExampleCauseForAction
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);

        }

        [Test]
        public void TestRequestForProductionIsValid03()
        {
            var testSubject = new RequestForProduction
            {
                Court = new StateCourt("AL"),
                GetSubjectPerson = lps => lps.Plaintiff(),
                GetAssertion = lp => lp is IPlaintiff ? new ExampleCauseForAction() : null,
                IsUnbalancedToNeedsOfCase = lc => lc is ExampleCauseForAction
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);

        }

        [Test]
        public void TestRequestForProductionIsValid04()
        {
            var testSubject = new RequestForProduction
            {
                Court = new StateCourt("AL"),
                GetSubjectPerson = lps => lps.Plaintiff(),
                GetAssertion = lp => lp is IPlaintiff ? new ExampleCauseForAction() : null,
                IsReasonablyAccessible = lc => !(lc is ExampleCauseForAction)
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);

        }
    }
}
