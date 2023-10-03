using System;
using NoFuture.Law.Property.US;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;

namespace NoFuture.Law.Tort.US.Elements
{
    /// <summary>
    /// When a trespasser makes improvements to another&apos;s property 
    /// </summary>
    public class ImprovingTrespassers : PropertyConsent
    {
        public ImprovingTrespassers() : base(ExtensionMethods.Defendant)
        {
        }

        /// <summary>
        /// when the improvements where on land the defendant believed to be their&apos;s
        /// </summary>
        public Predicate<IDefendant> IsTrespasserGoodFaithIntent { get; set; } = lp => false;

        /// <summary>
        /// improvements were not known to the owner until after their completion, not induced, not fraud, etc.
        /// </summary>
        public Predicate<IPlaintiff> IsOwnerGoodFaithOblivious { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var dTitle = defendant.GetLegalPersonTypeName();
            var pTitle = plaintiff.GetLegalPersonTypeName();

            if (!WithoutConsent(persons))
                return false;

            if (!IsTrespasserGoodFaithIntent(defendant))
            {
                AddReasonEntry($"{dTitle} {defendant.Name}, {nameof(IsTrespasserGoodFaithIntent)} is false");
                return false;
            }

            if (!IsOwnerGoodFaithOblivious(plaintiff))
            {
                AddReasonEntry($"{pTitle} {plaintiff.Name}, {nameof(IsOwnerGoodFaithOblivious)} is false");
                return false;
            }

            return true;
        }
    }
}
