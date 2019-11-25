using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests
{
    [TestFixture]
    public class ExampleTestFederalSubjectMatterJurisdiction
    {
        [Test]
        public void TestSubjectMatterJurisdictionIsValid()
        {
            var testSubject = new FederalSubjectMatterJurisdiction(new SomeOtherKindOfCourt("kangaroo"))
            {
                IsAuthorized2ExerciseJurisdiction = lc => lc is SomeFederalLegalMatter,
                IsArisingFromFederalLaw = lc => lc is SomeFederalLegalMatter,
                GetAssertion = lp => new SomeFederalLegalMatter()
            };
            var testResult = testSubject.IsValid();
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class SomeFederalLegalMatter : LegalConcept
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }
    }

    public class SomeOtherKindOfCourt : VocaBase, ICourt
    {
        public SomeOtherKindOfCourt(string name) : base(name) { }
    }
}
