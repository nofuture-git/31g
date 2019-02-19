using System;

namespace NoFuture.Rand.Law.Contract.US.Excuse
{
    /// <inheritdoc />
    public abstract class ExcuseBase<T> : DilemmaBase<T> where T : ILegalConcept
    {
        protected internal ExcuseBase(IContract<T> contract) : base(contract)
        {
        }

        public override bool IsEnforceableInCourt => true;

        /// <summary>
        /// <![CDATA[
        /// a party's performance is made impracticable without his fault
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsAtFault { get; set; } = lp => true;

        /// <summary>
        /// unless the language or the circumstances indicate the contrary
        /// </summary>
        public Predicate<ILegalPerson> IsContraryInForm { get; set; } = lp => false;

        protected internal bool IsPerformanceDischarged(ILegalPerson lp, params Predicate<ILegalPerson>[] furtherRules)
        {
            if (lp == null  || IsAtFault(lp) || IsContraryInForm(lp))
                return false;

            foreach (var predicate in furtherRules)
            {
                if (predicate(lp) == false)
                    return false;
            }

            AddReasonEntry($"{lp.Name} duty to render a performance is discharged.");
            AddReasonEntryRange(Contract.GetReasonEntries());
            return true;
        }
    }
}
