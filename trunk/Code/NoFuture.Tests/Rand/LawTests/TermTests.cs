using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests
{
    [TestFixture]
    public class TermTests
    {
        private static object something = new object();

        [Test]
        public void TestEquals()
        {
            var testSubj00 = new Term<object>("Swiss Coin Collection", new object());
            var testSubj01 = new Term<object>("Swiss Coin Collection", new object());

            Assert.IsTrue(testSubj00.Equals(testSubj01));
            Assert.IsFalse(testSubj00.EqualRefersTo(testSubj01));
            testSubj00 = new Term<object>("Swiss Coin Collection", something);
            testSubj01 = new Term<object>("Swiss Coin Collection", something);

            Assert.IsTrue(testSubj00.Equals(testSubj01));
            Assert.IsTrue(testSubj00.EqualRefersTo(testSubj01));

        }

        [Test]
        public void TestSetOps()
        {
            var set00 = new SortedSet<Term<object>>
            {
                new Term<object>("Swiss Coin Collection", new object()),
                new Term<object>("Rarity Coin Collection", new object())
            };

            var set01 = new SortedSet<Term<object>>
            {
                new Term<object>("Swiss Coin Collection", new object())
            };

            var intersect = set00.Where(oo => set01.Any(ee => oo.Equals(ee))).ToList();
            
            Assert.IsTrue(intersect.Any());
            Console.WriteLine(intersect.Count);

        }

        [Test]
        public void TestDecorator()
        {
            var testSubject = new Term<object>("test term", DBNull.Value);
            testSubject.As(new OralTerm()).As(new TechnicalTerm());

            var testResultName = testSubject.GetCategory();
            Console.WriteLine(testResultName);

            var testResult = testSubject.IsCategory(new OralTerm());
            Assert.IsTrue(testResult);

            testResult = testSubject.IsCategory(typeof(TechnicalTerm));
            Assert.IsTrue(testResult);

            testResult = testSubject.IsCategory(typeof(CommonUseTerm));
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestGetAdditionalTerms()
        {
            var terms00 = new HashSet<Term<object>>
            {
                new ContractTerm<object>("sell ranch", 0, new WrittenTerm()),
                new ContractTerm<object>("repurchase option", 1, new OralTerm())
            };
            var terms01 = new HashSet<Term<object>>
            {
                new ContractTerm<object>("sell ranch", 0, new WrittenTerm()),
            };

            var testResult = Term<object>.GetAdditionalTerms(terms00, terms01);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());

        }

        [Test]
        public void TestGetInNameAgreedTerms()
        {
            var terms00 = new HashSet<Term<object>>
            {
                new ContractTerm<object>("sell ranch", 0, new WrittenTerm()),
                new ContractTerm<object>("repurchase option", 1, new OralTerm())
            };
            var terms01 = new HashSet<Term<object>>
            {
                new ContractTerm<object>("sell ranch", 0, new WrittenTerm()),
            };

            var testResult = Term<object>.GetInNameAgreedTerms(terms00, terms01);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());
            Assert.IsTrue(testResult.Count == 1);
            Assert.AreEqual("sell ranch", testResult.First().Name);
        }

        [Test]
        public void TestGetAgreedTerms()
        {
            var terms00 = new HashSet<Term<object>>
            {
                new ContractTerm<object>("sell ranch", 0, new WrittenTerm()),
                new ContractTerm<object>("repurchase option", 1, new OralTerm()),
                new ContractTerm<object>("wrong value", 12, new OralTerm()),
            };
            var terms01 = new HashSet<Term<object>>
            {
                new ContractTerm<object>("sell ranch", 0, new WrittenTerm()),
                new ContractTerm<object>("wrong name", 1),
                new ContractTerm<object>("wrong value", 11, new OralTerm()),
            };

            var testResult = Term<object>.GetAgreedTerms(terms00, terms01);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());
            Assert.IsTrue(testResult.Count == 1);
            Assert.AreEqual("sell ranch", testResult.First().Name);
        }
    }
}
