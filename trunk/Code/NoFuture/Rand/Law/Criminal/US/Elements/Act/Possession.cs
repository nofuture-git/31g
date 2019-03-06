using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Act
{
    /// <summary>
    /// <![CDATA[Model Penal Code states in § 2.01(4)]]>
    /// </summary>
    /// <remarks>
    /// actual possession: possession as item on very person or very near
    /// constructive possession: not on person but within an area of control
    /// </remarks>
    public class Possession : CriminalBase, IPossession
    {
        public Predicate<ILegalPerson> IsKnowinglyProcured { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsKnowinglyReceived { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsAwareOfControlThereof { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsSufficientTimeToGetRid { get; set; } = lp => false;

        /// <inheritdoc cref="IActusReus"/>
        public virtual bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (IsKnowinglyProcured(defendant))
            {
                AddReasonEntry($"the defendant, {defendant.Name}, {nameof(IsKnowinglyProcured)} is true");
                return true;
            }

            if (IsKnowinglyReceived(defendant))
            {
                AddReasonEntry($"the defendant, {defendant.Name}, {nameof(IsKnowinglyReceived)} is true");
                return true;
            }

            if (IsAwareOfControlThereof(defendant) && IsSufficientTimeToGetRid(defendant))
            {
                AddReasonEntry($"the defendant, {defendant.Name}, {nameof(IsAwareOfControlThereof)} " +
                               $"and {nameof(IsSufficientTimeToGetRid)} are both true");
                return true;
            }

            return false;
        }
    }

}
