using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.Contract.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.MutualAssentTests
{
    /// <summary>
    /// Dr. Werner OSWALD, Plaintiff-Appellant, v. Jane B. ALLEN, Defendant-Appellee 417 F.2d 43; 1969 U.S. App.
    /// </summary>
    [TestFixture]
    public class OswaldvAllenTests
    {
        [Test]
        public void TestIsTermsOfAgreementValid()
        {
            var testSubject = new MutualAssent();

            testSubject.IsApprovalExpressed = lp => lp is DrOswald || lp is MrsAllen;
            testSubject.TermsOfAgreement = lp =>
            {
                var isParty = lp is DrOswald || lp is MrsAllen;
                if (!isParty)
                    return null;

                switch (lp)
                {
                    case DrOswald drOswald:
                        return drOswald.GetTerms();
                    case MrsAllen mrsAllen:
                        return mrsAllen.GetTerms();
                }

                return null;
            };

            var testResult = testSubject.IsValid(new MrsAllen(), new DrOswald());
            Assert.IsFalse(testResult);
            Console.WriteLine("--" + string.Join(",", testSubject.GetReasonEntries()));
        }
    }


    public class DrOswald : LegalPerson
    {
        public DrOswald() : base("Dr. Oswald") { }

        public ISet<Term<object>> GetTerms()
        {
            return new SortedSet<Term<object>>
            {
                new Term<object>("Swiss Coin Collection", new object())
            };
        }

    }

    public class MrsAllen : LegalPerson
    {
        public MrsAllen() : base("Mrs. Allen") { }

        public ISet<Term<object>> GetTerms()
        {
            return new SortedSet<Term<object>>
            {
                new Term<object>("Swiss Coin Collection", new object())
            };
        }
    }
}
