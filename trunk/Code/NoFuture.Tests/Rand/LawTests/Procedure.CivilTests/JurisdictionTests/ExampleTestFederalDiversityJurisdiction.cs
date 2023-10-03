using System;
using NoFuture.Law;
using NoFuture.Law.Procedure.Civil.US.Jurisdiction;
using NoFuture.Law.US.Courts;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Civil.Tests
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
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
            testSubject.ClearReasons();

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
            testSubject.ClearReasons();

            testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleForeigner());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }
    }

    public class ExampleForeigner : LegalPerson, IForeigner, IDefendant
    {
        public ExampleForeigner() : base("Fritz C Shlaphindagen") { }
    }
}
