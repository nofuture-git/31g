using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction;
using NoFuture.Rand.Law.US.Courts;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests
{

    [TestFixture]
    public class ExampleTestRemoval
    {
        [Test]
        public void TestRemovalIsValid()
        {
            var someCase = new FederalDiversityJurisdiction(new StateCourt("Missouri"))
            {
                GetDomicileLocation = lp =>
                {
                    if (lp is IPlaintiff)
                        return new VocaBase("Ohio");
                    if (lp is IDefendant)
                        return new VocaBase("Missouri");
                    return null;
                },
                GetInjuryClaimDollars = lp => 75000.01M

            };

            var testSubject = new Removal(someCase)
            {
                IsRequestRemoval = lp => lp is IDefendant
            };
            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }
}
