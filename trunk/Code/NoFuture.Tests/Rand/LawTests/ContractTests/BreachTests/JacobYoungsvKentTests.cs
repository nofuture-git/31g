using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Breach;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.BreachTests
{
    /// <summary>
    /// JACOB & YOUNGS, INC. v. KENT  Court of Appeals of New York 230 N.Y. 239, 129 N.E. 889 (1921)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, substantial performance is divides essential from trivial and innocent
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class JacobYoungsvKentTests
    {
        [Test]
        public void JacobYoungvKent()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferBuildCountryResidence(),
                Acceptance = o => o is OfferBuildCountryResidence ? new AcceptanctBuildCountryResidence() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case JacobYoung _:
                                return ((JacobYoung)lp).GetTerms();
                            case Kent _:
                                return ((Kent)lp).GetTerms();
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

            var testResult = testContract.IsValid(new JacobYoung(), new Kent());
            Assert.IsTrue(testResult);

            var testSubject = new SubstantialPerformance<Promise>(testContract)
            {
                ActualPerformance = lp =>
                {
                    if (lp is JacobYoung)
                        return new OfferBuildCountryResidence();
                    if (lp is Kent)
                        return new AcceptanctBuildCountryResidence
                        {
                            PipeToUse = new PlumbingPipe {Manufacturer = "some other companyu"}
                        };
                    return null;
                }
            };

            testResult = testSubject.IsValid(new JacobYoung(), new Kent());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class PlumbingPipe : LegalConcept
    {
        public string Material { get; set; } = "iron pipe";
        public string Surface { get; set; } = "galvanized";
        public string Weld { get; set; } = "lap welded";
        public string Grade { get; set; } = "standard pipe";
        public string Manufacturer { get; set; } = "Reading manufacture";
        public override bool Equals(object obj)
        {
            var pp = obj as PlumbingPipe;
            if (pp == null)
                return false;

            return EquivalentTo(obj) && string.Equals(pp.Manufacturer, Manufacturer);
        }

        public override int GetHashCode()
        {
            return (Material?.GetHashCode() ?? 1)
                   + (Surface?.GetHashCode() ?? 1)
                   + (Weld?.GetHashCode() ?? 1)
                   + (Grade?.GetHashCode() ?? 1)
                   + (Manufacturer?.GetHashCode() ?? 1);

        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return true;
        }

        /// <summary>
        /// <![CDATA[
        /// The court finds requiring the Manufacturer to match as "both trivial and innocent"
        /// ]]>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool EquivalentTo(object obj)
        {
            var pp = obj as PlumbingPipe;
            if (pp == null)
                return false;

            return string.Equals(pp.Material, Material) 
                   && string.Equals(pp.Surface, Surface)
                   && string.Equals(pp.Weld, Weld)
                   && string.Equals(pp.Grade, Grade);
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class OfferBuildCountryResidence : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is JacobYoung || offeror is Kent)
                   && (offeree is JacobYoung || offeree is Kent);
        }

        public PlumbingPipe PipeToUse { get; set; } = new PlumbingPipe();

        public decimal UpwardsCost => 77000m;

        public decimal RecoverUnpaid => 3483.46m;

        public override bool Equals(object obj)
        {
            var o = obj as OfferBuildCountryResidence;
            if (o == null)
                return false;

            return PipeToUse.EquivalentTo(o.PipeToUse);
        }
    }

    public class AcceptanctBuildCountryResidence : OfferBuildCountryResidence
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctBuildCountryResidence;
            if (o == null)
                return false;
            return true;
        }
    }

    public class JacobYoung : LegalPerson
    {
        public JacobYoung(): base("") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }

    public class Kent : LegalPerson
    {
        public Kent(): base("") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }
}
