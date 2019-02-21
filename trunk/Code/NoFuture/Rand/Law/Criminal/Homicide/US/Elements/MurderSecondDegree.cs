using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.Homicide.US.Elements
{
    /// <summary>
    /// often defined as any murder that is not <see cref="MurderFirstDegree"/>
    /// </summary>
    public class MurderSecondDegree : Murder
    {
        public Predicate<ILegalPerson> IsExtremeIndifferenceToHumanLife { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsExtremeIndifferenceToHumanLife(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsExtremeIndifferenceToHumanLife)} is false");
                return false;
            }
            return base.IsValid(persons);
        }

        public override bool CompareTo(IMensRea criminalIntent)
        {
            return true;
        }
    }
}
