using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction;
using NoFuture.Rand.Law.US.Courts;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests
{
    [TestFixture]
    public class ExampleTestFederalDiversityJurisdiction
    {
        [Test]
        public void TestFederalDiversityJurisdictionIsValid()
        {
            var testSubject = new FederalDiversityJurisdiction(new FederalCourt("some district"))
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

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            
            Assert.IsTrue(testResult);
            
            testSubject.Court = new StateCourt("Ohio");
            testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
            testSubject.ClearReasons();

            testSubject.Court = new FederalCourt("some district");
            testSubject.GetInjuryClaimDollars = lp => 74999.99M;
            testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
            testSubject.ClearReasons();

            testSubject.GetDomicileLocation = lp =>
            {
                if (lp is IPlaintiff)
                    return new VocaBase("Missouri");
                if (lp is IDefendant)
                    return new VocaBase("Missouri");
                return null;
            };
            testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);

        }
    }
}
