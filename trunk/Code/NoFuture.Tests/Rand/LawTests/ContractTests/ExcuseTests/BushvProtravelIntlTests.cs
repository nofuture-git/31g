using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Excuse;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.ExcuseTests
{
    /// <summary>
    /// BUSH v. PROTRAVEL INTERNATIONAL, INC. Civil Court of the City of New York 192 Misc. 2d 743, 746 N.Y.S.2d 790 (Civ.Ct. 2002)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the nature of "occurrence of an event" is the kind which no body would expect
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class BushvProtravelIntlTests
    {
        [Test]
        public void BushvProtravelIntl()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferTripToEastAfrica(),
                Acceptance = o => o is OfferTripToEastAfrica ? new AcceptanctTripToEastAfrica() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Bush _:
                                return ((Bush)lp).GetTerms();
                            case ProtravelIntl _:
                                return ((ProtravelIntl)lp).GetTerms();
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

            var testResult = testContract.IsValid(new Bush(), new ProtravelIntl());
            Assert.IsTrue(testResult);

            var testSubject = new ImpracticabilityOfPerformance<Promise>(testContract)
            {
                //the basic assumption being that NY NY would not suffer an attack
                IsBasicAssumptionGone = lp => lp is Bush,
                IsContraryInForm = lp => false,
                IsAtFault = lp => false
            };

            testResult = testSubject.IsValid(new Bush(), new ProtravelIntl());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferTripToEastAfrica : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Bush || offeror is ProtravelIntl)
                   && (offeree is Bush || offeree is ProtravelIntl);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferTripToEastAfrica;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctTripToEastAfrica : OfferTripToEastAfrica
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctTripToEastAfrica;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Bush : LegalPerson
    {
        public Bush(): base("ALEXANDRA BUSH") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("East Africa", DBNull.Value),
            };
        }
    }

    public class ProtravelIntl : LegalPerson
    {
        public ProtravelIntl(): base("PROTRAVEL INTERNATIONAL, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("East Africa", DBNull.Value),
            };
        }
    }
}
