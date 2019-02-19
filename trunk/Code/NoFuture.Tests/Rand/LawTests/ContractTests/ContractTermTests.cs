using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests
{
    [TestFixture]
    public class ContractTermTests
    {
        [Test]
        public void TestCompareTo()
        {
            var testSubject00 = new ContractTerm<object>("car parts", "car parts");
            var testSubject01 = new ContractTerm<object>("car parts", "car parts");

            var testResult = testSubject00.CompareTo(null);
            Assert.AreEqual(1, testResult);

            testResult = testSubject00.CompareTo(testSubject01);
            Assert.AreEqual(0, testResult);

            testSubject00.As(new WrittenTerm());
            testResult = testSubject00.CompareTo(testSubject01);
            Assert.AreEqual(-1, testResult);

            testResult = testSubject01.CompareTo(testSubject00);
            Assert.AreEqual(1, testResult);
        }

        [Test]
        public void TestCompareTo_StdPrefInter()
        {
            var testSubject00 = new ContractTerm<object>("car parts", "car parts", new ExpressTerm());
            var testSubject01 = new ContractTerm<object>("car parts", "car parts", new ExpressTerm());

            var testResult = testSubject00.CompareTo(testSubject01);
            Assert.AreEqual(0, testResult);

            testSubject00 = new ContractTerm<object>("car parts", "car parts", new ExpressTerm());
            testSubject01 = new ContractTerm<object>("car parts", "car parts", new CourseOfPerformanceTerm());

            testResult = testSubject00.CompareTo(testSubject01);
            Assert.IsTrue(testResult < 0);

            testResult = testSubject01.CompareTo(testSubject00);
            Assert.IsTrue(testResult > 0);


            testSubject00 = new ContractTerm<object>("car parts", "car parts", new CourseOfPerformanceTerm());
            testSubject01 = new ContractTerm<object>("car parts", "car parts", new CourseOfDealingTerm());

            testResult = testSubject00.CompareTo(testSubject01);
            Assert.IsTrue(testResult < 0);

            testResult = testSubject01.CompareTo(testSubject00);
            Assert.IsTrue(testResult > 0);

            testSubject00 = new ContractTerm<object>("car parts", "car parts", new CourseOfDealingTerm());
            testSubject01 = new ContractTerm<object>("car parts", "car parts", new UsageOfTradeTerm());

            testResult = testSubject00.CompareTo(testSubject01);
            Assert.IsTrue(testResult < 0);

            testResult = testSubject01.CompareTo(testSubject00);
            Assert.IsTrue(testResult > 0);
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

        [Test]
        public void TestThrowOnDupRefersTo()
        {
            var terms01 = new HashSet<Term<object>>
            {
                new ContractTerm<object>("first name", 0, new WrittenTerm()),
                new ContractTerm<object>("second name", 0),
                new ContractTerm<object>("term 00", 11, new OralTerm()),
                new ContractTerm<object>("term 01", 12, new OralTerm()),
                new ContractTerm<object>("term 02", 13, new OralTerm()),
            };
            Assert.Throws<AggregateException>(() => Term<object>.ThrowOnDupRefersTo(terms01));
        }
    }
}
