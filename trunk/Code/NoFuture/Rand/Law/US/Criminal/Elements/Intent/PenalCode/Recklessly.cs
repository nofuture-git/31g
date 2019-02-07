using System;

namespace NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode
{
    /// <summary>
    /// <![CDATA[
    /// engage in conduct being aware of the nature and the substantial and unjustifiable risk
    /// see (Model Penal Code § 2.02(2)
    /// ]]>
    /// </summary>
    public class Recklessly: MensRea, IComparable
    {
        /// <summary>
        /// consciously disregard a substantial risk of harm
        /// </summary>
        public Predicate<ILegalPerson> IsDisregardOfRisk { get; set; } = lp => false;

        /// <summary>
        /// a reasonable person would not take the risk
        /// </summary>
        public Predicate<ILegalPerson> IsUnjustifiableRisk { get; set; } = lp => false;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            if (!IsDisregardOfRisk(defendant))
            {
                AddReasonEntry($"the defendant {defendant.Name} DID regard the risk");
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
            if (obj is Purposely || obj is Knowingly)
                return -1;
            if (obj is Negligently)
                return 1;
            return 0;
        }
    }
}
