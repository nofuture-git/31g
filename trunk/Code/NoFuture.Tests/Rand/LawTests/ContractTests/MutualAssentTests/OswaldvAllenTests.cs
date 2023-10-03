using System;
using System.Collections.Generic;
using NoFuture.Law.Contract.US;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Contract.Tests.MutualAssentTests
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


    public class DrOswald : LegalPerson, IOfferee
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

    public class MrsAllen : LegalPerson, IOfferor
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
