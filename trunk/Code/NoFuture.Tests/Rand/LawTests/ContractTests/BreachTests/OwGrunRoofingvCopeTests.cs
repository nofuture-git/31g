using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Breach;
using NoFuture.Rand.Law.Contract.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.BreachTests
{
    /// <summary>
    /// O. W. GRUN ROOFING & CONSTRUCTION CO. v. COPE Court of Civil Appeals of Texas, Fourth District—San Antonio 529 S.W.2d 258 (Tex.Civ.App. 1975)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, substantial performance may include taste & preferences in addition to pure function & purpose
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class OwGrunRoofingvCopeTests
    {
        [Test]
        public void OwGrunRoofingvCope()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferInstallNewRoof(),
                Acceptance = o => o is OfferInstallNewRoof ? new AcceptanctInstallNewRoof() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case OwGrunRoofing _:
                                return ((OwGrunRoofing)lp).GetTerms();
                            case Cope _:
                                return ((Cope)lp).GetTerms();
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

            var testResult = testContract.IsValid(new OwGrunRoofing(), new Cope());
            Assert.IsTrue(testResult);

            var testSubject = new SubstantialPerformance<Promise>(testContract)
            {
                ActualPerformance = lp =>
                {
                    if (lp is OwGrunRoofing)
                        return new OfferInstallNewRoof {IsServePurposeOfRoof = true, IsUniformColor = false};
                    if (lp is Cope)
                        return new AcceptanctInstallNewRoof();
                    return null;
                }
            };

            testResult = testSubject.IsValid(new OwGrunRoofing(), new Cope());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }
    public class OfferInstallNewRoof : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is OwGrunRoofing || offeror is Cope)
                   && (offeree is OwGrunRoofing || offeree is Cope);
        }

        public bool IsServePurposeOfRoof { get; set; } = true;
        public bool IsUniformColor { get; set; } = true;

        public override bool EquivalentTo(object obj)
        {
            var o = obj as OfferInstallNewRoof;
            if (o == null)
                return false;
            return o.IsServePurposeOfRoof == IsServePurposeOfRoof
                   && o.IsUniformColor == IsUniformColor
                ;
        }
    }

    public class AcceptanctInstallNewRoof : OfferInstallNewRoof
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctInstallNewRoof;
            if (o == null)
                return false;
            return true;
        }
    }

    public class OwGrunRoofing : LegalPerson
    {
        public OwGrunRoofing() : base("O. W. GRUN ROOFING & CONSTRUCTION CO.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("roof shingles", DBNull.Value),
                new ContractTerm<object>("russet glow", 2),
            };
        }
    }

    public class Cope : LegalPerson
    {
        public Cope() : base("MRS. FRED M. COPE") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("roof shingles", DBNull.Value),
                new ContractTerm<object>("russet glow", 2),
            };
        }
    }
}
