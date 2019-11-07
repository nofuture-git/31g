using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction;
using NoFuture.Rand.Law.US.Courts;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests
{
    [TestFixture]
    public class ExampleTestSubjectMatterJurisdiction
    {
        [Test]
        public void TestSubjectMatterJurisdictionIsValid()
        {
            var testSubject = new SubjectMatterJurisdiction(new SomeOtherKindOfCourt("kangaroo"))
            {
                IsAuthorized2ExerciseJurisdiction = lc => lc is SomeFederalLegalMatter,
                IsArisingFromFederalLaw = lc => lc is SomeFederalLegalMatter,
                CausesOfAction = new SomeFederalLegalMatter()
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
