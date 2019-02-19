using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Semiosis;
using NoFuture.Rand.Law.Contract.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.SemiosisTests
{
    /// <summary>
    /// IN RE ESTATE OF SOPER Supreme Court of Minnesota 196 Minn. 60, 264 N.W. 427 (1935)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, what does the court do when a common use word has two possible meanings
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class InReEstateOfSoperTests
    {
        [Test]
        public void InReEstateOfSoper()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new InheritSoperEstate(),
                Acceptance = o => o is InheritSoperEstate ? new GetsSoperEstate() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case IraSoper _:
                                return ((IraSoper)lp).GetTerms();
                            case GertrudeWhitby _:
                                return ((GertrudeWhitby)lp).GetTerms();
                            case AdelineWestphal _:
                                return ((AdelineWestphal)lp).GetTerms();
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

            //although legally his Gertrude is his legal wife - the contract's intend was obviously Adeline
            var testSubject = new SemanticDilemma<Promise>(testContract)
            {
                IsIntendedMeaningAtTheTime = t => t.RefersTo is AdelineWestphal
            };

            var testResult = testSubject.IsValid(new IraSoper(), new GertrudeWhitby());
            Assert.IsTrue(testResult);
            testResult = testSubject.IsValid(new IraSoper(), new AdelineWestphal());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class InheritSoperEstate : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is IraSoper) && (offeree is GertrudeWhitby || offeree is AdelineWestphal);
        }
    }

    public class GetsSoperEstate : InheritSoperEstate { }

    public class IraSoper : LegalPerson
    {
        public IraSoper() : base("John Young")
        {
            Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Former, "Ira Soper"));
        }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("wife", new AdelineWestphal(), new WrittenTerm(), new ExpressTerm()),
            };
        }

    }

    public class GertrudeWhitby : LegalPerson
    {
        public GertrudeWhitby() : base("Gertrude Whitby") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("wife", this, new WrittenTerm(), new ExpressTerm()),
            };
        }
    }

    public class AdelineWestphal : LegalPerson
    {
        public AdelineWestphal() : base("Adeline Westphal") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("wife", this, new WrittenTerm(), new ExpressTerm()),
            };
        }
    }

}
