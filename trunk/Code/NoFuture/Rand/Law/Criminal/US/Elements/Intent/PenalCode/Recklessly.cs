using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.Intent.PenalCode
{
    /// <summary>
    /// <![CDATA[
    /// consciously disregard a substantial risk of harm
    /// see (Model Penal Code § 2.02(2)
    /// ]]>
    /// </summary>
    [Aka("implied malice", "depraved heart")]
    public class Recklessly: MensRea
    {
        /// <summary>
        /// Conduct that is short of purposeful intent to cause harm.
        /// </summary>
        public Predicate<ILegalPerson> IsDisregardOfRisk { get; set; } = lp => false;

        /// <summary>
        /// a reasonable person would not take the risk
        /// </summary>
        public Predicate<ILegalPerson> IsUnjustifiableRisk { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!IsDisregardOfRisk(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name} {nameof(IsDisregardOfRisk)} is false");
                return false;
            }

            if (!IsUnjustifiableRisk(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name} {nameof(IsUnjustifiableRisk)} is false");
                return false;
            }

            return true;
        }

        public override int CompareTo(object obj)
        {
            if (obj is Purposely || obj is Knowingly)
                return -1;
            if (obj is Negligently)
                return 1;
            return 0;
        }
    }
}
