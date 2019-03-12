using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Defense.ToFormation;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.DefenseTests
{
    /// <summary>
    /// ORTELERE v. TEACHERS’ RETIREMENT BOARD New York Court of Appeals 25 N.Y.2d 196, 250 N.E.2d 460, 303 N.Y.S.2d 362 (1969)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, being considered mentally incompetent makes a contract voidable
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class OrtelerevTeachersBoardTests
    {
        [Test]
        public void OrtelerevTeachersBoard()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new MaxInLifetimeOption(),
                Acceptance = o => o as MaxInLifetimeOption,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => Ortelere.GetTerms()
                }
            };
            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByOfferee = (lp, p) => true,
                IsSoughtByOfferor = (lp, p) => true
            };
            var testSubject = new ByMentalIncompetent<Promise>(testContract) {IsMentallyIncompetent = lp => lp is Ortelere};
            var testResult = testSubject.IsValid(new Ortelere(), new TeachersBoard());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class MaxInLifetimeOption : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }
    }

    public class Ortelere : LegalPerson, IOfferor
    {
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("pension", DBNull.Value)
            };
        }

    }

    public class TeachersBoard : LegalPerson, IOfferee
    {
        public TeachersBoard() : base("TEACHERS’ RETIREMENT BOARD") { }
    }
}
