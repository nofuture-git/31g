using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Defense;
using NoFuture.Rand.Law.US.Contracts.Defense.ToFormation;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.DefenseTests
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
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };
            var testSubject = new ByMentalIncompetent<Promise>(testContract) {IsMentallyIncompetent = lp => lp is Ortelere};
            var testResult = testSubject.IsValid(new Ortelere(), new TeachersBoard());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class MaxInLifetimeOption : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return true;
        }
    }

    public class Ortelere : LegalPerson
    {
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("pension", DBNull.Value)
            };
        }

    }

    public class TeachersBoard : LegalPerson
    {
        public TeachersBoard() : base("TEACHERS’ RETIREMENT BOARD") { }
    }
}
