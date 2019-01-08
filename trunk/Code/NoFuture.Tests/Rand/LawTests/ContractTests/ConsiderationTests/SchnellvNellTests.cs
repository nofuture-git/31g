using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.ConsiderationTests
{
    /// <summary>
    /// SCHNELL v. NELL Supreme Court of Indiana 17 Ind. 29 (1861)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue: the consideration was really a indeterminate value and 
    /// the "one-cent" value was just a nominal standin to make it look like
    /// a legal contract when its really just a social one.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class SchnellvNellTests
    {
        [Test]
        public void SchnellvNell()
        {
            var testSubject = new BilateralContract()
            {
                Offer = new OfferOneCent(),
                Acceptance = o => o is OfferOneCent ? new AcceptancePaySixHundred() : null,
                MutualAssent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => new HashSet<Term<object>> {new Term<object>("undefined", DBNull.Value)}
                }
            };

            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {
                IsSoughtByPromisor = (lp, p) => true,
                IsGivenByPromisee = (lp, p) => true
            };

            var testResult = testSubject.IsValid(new Schnell(), new Nell());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferOneCent : SocialContract
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            AddAuditEntry("this fails consideration because one " +
                          "cent is not any real value in eyes of law");
            return false;
        }
    }

    public class AcceptancePaySixHundred : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is Schnell && offeree is Nell;
        }
    }

    public class Schnell : VocaBase, ILegalPerson
    {
        public Schnell() : base("Zacharias Schnell") { }
    }

    public class Nell : VocaBase, ILegalPerson
    {
        public Nell() : base("J. B. Nell") { }
    }

}
