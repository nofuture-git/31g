using System;
using NoFuture.Law;
using NoFuture.Law.Procedure.Civil.US.Jurisdiction;
using NoFuture.Law.US.Courts;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Civil.Tests
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
