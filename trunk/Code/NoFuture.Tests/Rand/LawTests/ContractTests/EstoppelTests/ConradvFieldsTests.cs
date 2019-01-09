using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.EstoppelTests
{
    /// <summary>
    /// CONRAD v. FIELDS Court of Appeals of Minnesota 2007 Minn.App.Unpub.LEXIS 744
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, promissory estoppel attempts to return the relying promisee 
    /// back into pre-contractual state
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ConradvFieldsTests
    {
        [Test]
        public void ConradvFields()
        {
            var testSubject = new LegalContract<Promise>()
            {
                Offer = new OfferPayForLawSchool()
            };
            testSubject.Consideration = new PromissoryEstoppel<Promise>(testSubject)
            {
                IsOffereeDependedOnPromise = lp => true,
                IsOffereePositionWorse = lp => true
            };
            var testResult = testSubject.IsValid(new Fields(), new Conrad());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferPayForLawSchool : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is Fields && offeree is Conrad;
        }
    }

    public class Conrad : VocaBase, ILegalPerson
    {
        public Conrad() : base("Marjorie Conrad") { }
    }

    public class Fields : VocaBase, ILegalPerson
    {
        public Fields() : base("Walter R. Fields") { }
    }
}
