using System;

namespace NoFuture.Rand.Law.US.Criminal.Elements
{
    /// <summary>
    /// <![CDATA[Model Penal Code states in § 2.01(4)]]>
    /// </summary>
    /// <remarks>
    /// actual possession: possession as item on very person or very near
    /// constructive possession: not on person but within an area of control
    /// </remarks>
    public class Possession : ActusReus
    {
        public Predicate<ILegalPerson> IsKnowinglyProcured { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsKnowinglyReceived { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsAwareOfControlThereof { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsSufficientTimeToGetRid { get; set; } = lp => false;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            if (IsKnowinglyProcured(defendant))
            {
                AddReasonEntry($"the defendant, {defendant.Name}, is possession knowingly procured");
                return true;
            }

            if (IsKnowinglyReceived(defendant))
            {
                AddReasonEntry($"the defendant, {defendant.Name}, is possession knowingly received");
                return true;
            }

            if (IsAwareOfControlThereof(defendant) && IsSufficientTimeToGetRid(defendant))
            {
                AddReasonEntry($"the defendant, {defendant.Name}, is possession aware of control and given time to dispose");
                return true;
            }

            return false;
        }
    }

}
