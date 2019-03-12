using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Semiosis;
using NoFuture.Rand.Law.Contract.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.SemiosisTests
{
    /// <summary>
    /// W.W.W. ASSOCIATES, INC. v. GIANCONTIERI Court of Appeals of New York 77 N.Y.2d 157, 566 N.E.2d 639, 565 N.Y.S.2d 440 (1990)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the expressly conditional term may exclude terms that are are written but ancillary
    /// "extrinsic and parol evidence is not admissible to create an ambiguity in a written agreement which is complete and clear and unambiguous upon its face."
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class WwwAssociatesvGiancontieriTests
    {
        [Test]
        public void WwwAssociatesvGiancontieri()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferWwwAssociates(),
                Acceptance = o => o is OfferWwwAssociates ? new AcceptanceWwwAssociates() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case WwwAssociates _:
                                return ((WwwAssociates)lp).GetTerms();
                            case Giancontieri _:
                                return ((Giancontieri)lp).GetTerms();
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

            var testSubject = new SemanticDilemma<Promise>(testContract)
            {
                //since this was present
                IsPrerequisite = t => t is ExpresslyConditionalTerm,
                IsIntendedMeaningAtTheTime = t =>
                {
                    // and the contract clear said "either party can cancel" after a specific date
                    var isDateTime = t.RefersTo is DateTime;
                    if (!isDateTime)
                        return false;
                    var tDt = (DateTime) t.RefersTo;
                    return tDt != DateTime.MaxValue;
                }
            };
            var testResult = testSubject.IsValid(new WwwAssociates(), new Giancontieri());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }
    }

    public class OfferWwwAssociates : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is WwwAssociates || offeror is Giancontieri)
                   && (offeree is WwwAssociates || offeree is Giancontieri);
        }
    }

    public class AcceptanceWwwAssociates : OfferWwwAssociates { }

    public class WwwAssociates : LegalPerson, IOfferor
    {
        public WwwAssociates(): base("W.W.W. ASSOCIATES, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                ExpresslyConditionalTerm.Value,
                new ContractTerm<object>("cancel this contract", DateTime.MaxValue, new WrittenTerm(), new ExpressTerm()),
            };
        }
        public bool WantsToCancelContractAfterDate => false;
    }

    public class Giancontieri : LegalPerson, IOfferee
    {
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                ExpresslyConditionalTerm.Value,
                new ContractTerm<object>("cancel this contract", new DateTime(1987,6,1), new WrittenTerm(), new ExpressTerm())
            };
        }

        public bool WantsToCancelContractAfterDate => true;
    }
}
