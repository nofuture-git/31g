using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.Contract.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.ConsiderationTests
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
            var testSubject = new ComLawContract<Promise>
            {
                Offer = new OfferOneCent(),
                Acceptance = o => o is OfferOneCent ? new AcceptancePaySixHundred() : null,
                Assent = new MutualAssent
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

    public class OfferOneCent : DonativePromise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            AddReasonEntry("this fails consideration because one " +
                          "cent is not any real value in eyes of law");
            return false;
        }
    }

    public class AcceptancePaySixHundred : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is Schnell && offeree is Nell;
        }
    }

    public class Schnell : LegalPerson
    {
        public Schnell() : base("Zacharias Schnell") { }
    }

    public class Nell : LegalPerson
    {
        public Nell() : base("J. B. Nell") { }
    }

}
