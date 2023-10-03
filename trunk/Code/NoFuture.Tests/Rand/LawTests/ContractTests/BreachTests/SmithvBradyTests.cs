using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Law.Contract.US;
using NoFuture.Law.Contract.US.Breach;
using NoFuture.Law.Contract.US.Terms;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Contract.Tests.BreachTests
{
    /// <summary>
    /// SMITH v. BRADY Court of Appeals of New York 17 N.Y. 173 (1858)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, In other words, if you know what you are supposed to do, 
    /// your failure to do what you know you are supposed to do cannot be 
    /// "innocent" in the sense the court is using it.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class SmithvBradyTests
    {
        [Test]
        public void SmithvBrady()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferBuildSomeCottages(),
                Acceptance = o => o is OfferBuildSomeCottages ? new AcceptanceBuildSomeCottages() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Smith _:
                                return ((Smith)lp).GetTerms();
                            case Brady _:
                                return ((Brady)lp).GetTerms();
                            default:
                                return null;
                        }
                    }
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByOfferee = (lp, p) => true,
                IsSoughtByOfferor = (lp, p) => true
            };

            var testResult = testContract.IsValid(new Smith(), new Brady());
            Assert.IsTrue(testResult);

            var testSubject = new PerfectTender<Promise>(testContract)
            {
                ActualPerformance = lp =>
                {
                    if (lp is Smith)
                    {
                        return new AcceptanceBuildSomeCottages {InchesBetweenJoists = 16, InchesBetweenBeams = 24};
                    }
                    if(lp is Brady)
                        return new OfferBuildSomeCottages();
                    return null;
                }
            };

            testResult = testSubject.IsValid(new Smith(), new Brady());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferBuildSomeCottages : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Smith || offeror is Brady)
                   && (offeree is Smith || offeree is Brady);
        }

        public decimal Price { get; } = 4900m;

        public override bool Equals(object obj)
        {
            var o = obj as OfferBuildSomeCottages;
            if (o == null)
                return false;
            return o.Price == Price;
        }

        public override int GetHashCode()
        {
            return Price.GetHashCode();
        }
    }

    public class AcceptanceBuildSomeCottages : OfferBuildSomeCottages
    {
        public int InchesBetweenJoists { get; set; } = 12;
        public int InchesBetweenBeams { get; set; } = 16;

        public override bool Equals(object obj)
        {
            var o = obj as AcceptanceBuildSomeCottages;
            if (o == null)
                return false;
            return o.InchesBetweenJoists == InchesBetweenJoists && o.InchesBetweenBeams == InchesBetweenBeams;
        }

        public override int GetHashCode()
        {
            return InchesBetweenBeams.GetHashCode() + InchesBetweenJoists.GetHashCode();
        }
    }

    public class Smith : LegalPerson, IOfferor
    {
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("build cottages", DBNull.Value),
            };
        }
    }

    public class Brady : LegalPerson, IOfferee
    {
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("build cottages", DBNull.Value),
            };
        }
    }
}
