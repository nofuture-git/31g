using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US.Contracts.Terms;

namespace NoFuture.Rand.Law.US.Contracts.Semiosis
{
    /// <summary>
    /// <![CDATA[
    /// how much credit to give the writing in a contract over oral 
    /// ]]>
    /// </summary>
    [Aka("oral evidence rule")]
    [Note("from Old French for 'oral'", "not the same criminal term applied to convicts - 'parole'")]
    public class ParolEvidenceRule<T> : DilemmaBase<T> where T : IObjectiveLegalConcept
    {
        public ParolEvidenceRule(IContract<T> contract) : base(contract)
        {
        }

        public Predicate<Term<object>> IsCollateralInForm { get; set; } = r => false;

        public Predicate<Term<object>> IsNotContradictWritten { get; set; } = r => false;

        public Predicate<Term<object>> IsNotExpectedWritten { get; set; } = r => false;

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!TryGetTerms(offeror, offeree))
            {
                AddReasonEntry("parol evidence rule requires a contract with assent");
                return false;
            }

            var agreedTerms = AgreedTerms;
            if (agreedTerms != null && agreedTerms.Any(t => t is ExpresslyConditionalTerm))
            {
                AddReasonEntryRange(Contract.Assent.GetReasonEntries());
                AddReasonEntry("additional terms cannot be included since one " +
                               "of the written terms expressly states, " +
                               "additional terms cannot be included.");
                return false;
            }

            var additionalTerms = Contract.Assent.GetAdditionalTerms(offeror, offeree);
            if (additionalTerms == null || !additionalTerms.Any())
            {
                AddReasonEntryRange(Contract.Assent.GetReasonEntries());
                return false;
            }

            var oralAdditionalTerms = additionalTerms.Where(t => t.IsCategory(new OralTerm()))
                .ToList();

            if (!oralAdditionalTerms.Any())
            {
                AddReasonEntry("none of the additional terms are oral in source");
                return false;
            }

            foreach (var ot in oralAdditionalTerms)
            {
                if(ot == null)
                    continue;
                if (!IsCollateralInForm(ot))
                {
                    AddReasonEntry($"the term '{ot.Name}' is not collateral in form");
                    return false;
                }

                if (!IsNotContradictWritten(ot))
                {
                    AddReasonEntry($"the term '{ot.Name}' is a contradiction of what is in writing");
                    return false;
                }

                if (!IsNotExpectedWritten(ot))
                {
                    AddReasonEntry($"the term '{ot.Name}' is expected to have been written");
                    return false;
                }
            }

            return true;
        }
    }
}
