using System;

namespace NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode
{
    /// <summary>
    /// <![CDATA[
    /// should be aware of risk of harm
    /// see (Model Penal Code § 2.02(2) (d))
    /// ]]>
    /// </summary>
    public class Negligently : MensRea, IComparable
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

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            if (!IsUnawareOfRisk(defendant))
            {
                AddReasonEntry($"the defendant {defendant.Name} was aware of the risk");
                return false;
            }

            if (!IsUnjustifiableRisk(defendant))
            {
                AddReasonEntry($"the defendant {defendant.Name} took a justifiable risk");
                return false;
            }

            return true;
        }

        public virtual int CompareTo(object obj)
        {
            if (obj is Purposely || obj is Knowingly || obj is Recklessly)
                return -1;
            return 0;
        }
    }
}
