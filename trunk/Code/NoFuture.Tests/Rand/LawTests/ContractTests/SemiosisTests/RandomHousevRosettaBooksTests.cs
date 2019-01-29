using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Semiosis;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.SemiosisTests
{
    /// <summary>
    /// RANDOM HOUSE, INC. v. ROSETTA BOOKS LLC United States District Court for the Southern District of New York 150 F.Supp. 2d 613 (S.D.N.Y. 2001)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, semantic dilemma with "new use" problems
    /// new use: licensees may exploit licensed works through new marketing channels made possible by technologies developed after the licensing contract
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class RandomHousevRosettaBooksTests
    {
        [Test]
        public void RandomHousvRosettaBooks()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferSellBooks(),
                Acceptance = o => o is OfferSellBooks ? new AcceptanceSellBooks() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case AnyAuthorOfBook _:
                                return ((AnyAuthorOfBook)lp).GetTerms();
                            case RandomHouse _:
                                return ((RandomHouse)lp).GetTerms();
                            case RosettaBooks _:
                                return ((RosettaBooks)lp).GetTerms();
                            default:
                                return null;
                        }
                    }
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };

            var testResult = testContract.IsValid(new AnyAuthorOfBook(), new RandomHouse());
            Assert.IsTrue(testResult);
            testResult = testContract.IsValid(new AnyAuthorOfBook(), new RosettaBooks());
            Assert.IsTrue(testResult);

            var testSubject = new SemanticDilemma<Promise>(testContract)
            {
                IsIntendedMeaningAtTheTime = IsIntendedMeaning
            };

            testResult = testSubject.IsValid(new AnyAuthorOfBook(), new RandomHouse());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestIntentPredicate()
        {
            var testResult = IsIntendedMeaning(new ContractTerm<object>("in book form", new RandomHouseIdeaOfBook()));
            Console.WriteLine(testResult);
            testResult = IsIntendedMeaning(new ContractTerm<object>("in book form", new PrintedBook()));
            Console.WriteLine(testResult);
        }

        public static bool IsIntendedMeaning(Term<object> t)
        {
            return t.Name == "in book form" && t.RefersTo is IInPrintedForm && !(t.RefersTo is IInElectronicForm);
        }
    }

    public class OfferSellBooks : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is AnyAuthorOfBook && (offeree is RandomHouse || offeree is RosettaBooks);
        }
    }

    public class AcceptanceSellBooks : OfferSellBooks { }

    public class AnyAuthorOfBook : LegalPerson
    {
        public AnyAuthorOfBook():base("BOOK AUTHOR") { }

        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("sell books", DBNull.Value),
                new ContractTerm<object>("in book form", new PrintedBook()),
            };
        }
    }

    public class RandomHouse : LegalPerson
    {
        public RandomHouse(): base("RANDOM HOUSE, INC.") { }

        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("sell books", DBNull.Value),
                new ContractTerm<object>("in book form", new RandomHouseIdeaOfBook())
            };
        }
    }

    public class RosettaBooks : LegalPerson
    {
        public RosettaBooks(): base("ROSETTA BOOKS LLC") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("sell books", DBNull.Value),
                new ContractTerm<object>("in book form", new PrintedBook())
            };
        }
    }

    public interface ILiteraryWork { }
    public interface IInPrintedForm { }
    public interface IInElectronicForm { }

    public class Book : ILiteraryWork
    {
        public override bool Equals(object obj)
        {
            return obj is Book;
        }

        public override int GetHashCode()
        {
            return 2;
        }
    }

    public class PrintedBook : Book, IInPrintedForm
    {
        public override bool Equals(object obj)
        {
            return obj is IInPrintedForm;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + 2;
        }
    }

    public class EBook : Book, IInElectronicForm
    {
        public override bool Equals(object obj)
        {
            return obj is IInElectronicForm;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + 3;
        }
    }

    public class RandomHouseIdeaOfBook : Book, IInElectronicForm, IInPrintedForm
    {
        public override bool Equals(object obj)
        {
            return obj is RandomHouseIdeaOfBook;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + 3;
        }
    }
}
