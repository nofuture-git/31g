using System;
using System.Collections.Generic;
using NoFuture.Law.Procedure.Criminal.US.Challenges;
using NoFuture.Law.Procedure.Criminal.US.SearchReasons;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleChallengeToProbableCauseTests
    {
        [Test]
        public void TestChallengeToProbableCauseIsValid00()
        {
            var testSubject = new ChallengeToProbableCause<string>
            {
                Affidavit = new ExampleAffidavit(),
                IsDisregardOfTruth = af => true,
                IsContainFalseStatement = af => af.FactsThereof.Contains("threw a hammer"),
                IsFalseStatementCritical = af => true
            };

            var testResult = testSubject.IsValid(new ExampleBadCop(), new ExampleNotaryPublic());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class ExampleBadCop : ExampleLawEnforcement, IAffiant { }

    public class ExampleAffidavit : Affidavit<string>
    {
        public ExampleAffidavit()
        {
            FactsThereof = new List<string>
            {
                "threw a hammer"
            };
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
