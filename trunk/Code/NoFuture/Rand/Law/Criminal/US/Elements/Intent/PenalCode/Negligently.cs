using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.Intent.PenalCode
{
    /// <summary>
    /// <![CDATA[
    /// should be aware of risk of harm
    /// see (Model Penal Code § 2.02(2) (d))
    /// ]]>
    /// </summary>
    public class Negligently : MensRea
    {
        /// <summary>
        /// The difference between <see cref="Recklessly"/> is the person unknowingly takes a 
        /// risk - even though a reasonable person would have been aware of it.
        /// </summary>
        public Predicate<ILegalPerson> IsUnawareOfRisk { get; set; } = lp => false;

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
            if (!IsUnawareOfRisk(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name} {nameof(IsUnawareOfRisk)} is false");
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
            if (obj is Purposely || obj is Knowingly || obj is Recklessly)
                return -1;
            return 0;
        }
    }
}
