using System;
using System.Linq;
using NoFuture.Rand.Law.Procedure.Criminal.US;
using NoFuture.Rand.Law.Procedure.Criminal.US.Intrusions;
using NoFuture.Rand.Law.Procedure.Criminal.US.SearchReasons;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleReasonableSuspicionTests
    {
        [Test]
        public void TestReasonableSuspicionIsValid00()
        {
            var testSubject = new ReasonableSuspicion
            {
                IsFactsConcludeToCriminalActivity = lp => lp is ExampleLawEnforcement,
                Stop = new SuspectStop
                {
                    IsBeliefFreeToGo = lp => false,
                    GetDetainedTimespan = lp => new TimeSpan(0, 15, 0),
                    GetRequiredInvestigateTimespan = lp => new TimeSpan(0, 20, 0)
                }
            };

            var testResult =
                testSubject.IsValid(new ExampleSuspect(), new ExampleLawEnforcement(), new ExampleInformant());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        [Test]
        public void TestReasonableSuspicionIsValid01()
        {
            var testSubject = new ReasonableSuspicion
            {
                GetInformationSource = lps => lps.FirstOrDefault(lp => lp is IInformant),
                IsFactsConcludeToCriminalActivity = lp => lp is ExampleLawEnforcement,
                Stop = new SuspectStop
                {
                    IsBeliefFreeToGo = lp => false,
                    GetDetainedTimespan = lp => new TimeSpan(0, 15, 0),
                    GetRequiredInvestigateTimespan = lp => new TimeSpan(0, 20, 0)
                }
            };

            var testResult =
                testSubject.IsValid(new ExampleSuspect(), new ExampleLawEnforcement(),
                    new ExampleInformant {IsInformationSufficientlyReliable = true});
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestReasonableSuspicionIsValid02()
        {
            var testSubject = new ReasonableSuspicion
            {
                GetInformationSource = lps => lps.FirstOrDefault(lp => lp is ExampleLawEnforcement),
                IsFactsConcludeToCriminalActivity = lp => lp is ExampleLawEnforcement,
                Stop = new SuspectStop
                {
                    IsBeliefFreeToGo = lp => false,
                    GetDetainedTimespan = lp => new TimeSpan(3, 0, 0),
                    GetRequiredInvestigateTimespan = lp => new TimeSpan(0, 20, 0)
                }
            };

            var testResult =
                testSubject.IsValid(new ExampleSuspect(), new ExampleLawEnforcement(),
                    new ExampleInformant { IsInformationSufficientlyReliable = true });
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }
}
