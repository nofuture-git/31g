using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US.Terms;
using NoFuture.Rand.Law.Contract.US.Ucc;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.UccTests
{
    /// <summary>
    /// COMMERCE & INDUSTRY INS. CO. v. BAYER CORP. Supreme Judicial Court of Massachusetts 433 Mass. 388, 742 N.E.2d 567 (2001)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, detailed break-down of UCC 2-207 contract formation
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class CommerceInsvBayerCorpTests
    {
        [Test]
        public void CommerceInsvBayerCorp()
        {
            var testSubject = new Agreement
            {
                IsApprovalExpressed = lp => true,
                TermsOfAgreement = lp =>
                {
                    var commerceIns = lp as CommerceIns;
                    if (commerceIns != null)
                        return commerceIns.GetTerms();
                    var bayer = lp as BayerCorp;
                    if (bayer != null)
                        return bayer.GetTerms();
                    return new HashSet<Term<object>>();
                }
            };
            var testResult = testSubject.GetAgreedTerms(new CommerceIns(), new BayerCorp());
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Count == 1);
            //Bayer wanted the "arbitration provision" but its gone
            Assert.IsTrue(testResult.Any(t => t.Name == "bulk nylon fiber"));
        }
    }

    public class CommerceIns : LegalPerson
    {
        public CommerceIns() : base("COMMERCE & INDUSTRY INS. CO.") { }

        public ISet<Term<object>> GetTerms()
        {
            var terms = new HashSet<Term<object>>();
            terms.Add(new Term<object>("arbitration provision", new object()));
            terms.Add(new Term<object>("purchase order", new object()));
            terms.Add(new Term<object>("bulk nylon fiber", DBNull.Value));

            return terms;
        }
    }

    public class BayerCorp : LegalPerson
    {
        public BayerCorp() : base("BAYER CORP.") { }

        public ISet<Term<object>> GetTerms()
        {
            var terms = new HashSet<Term<object>>();
            terms.Add(new Term<object>("invoice", new object()));
            terms.Add(ExpresslyConditionalTerm.Value);
            terms.Add(new Term<object>("bulk nylon fiber", DBNull.Value));

            return terms;
        }
    }
}
