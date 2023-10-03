using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.Homicide
{
    /// <summary>
    /// often defined as any murder that is not <see cref="MurderFirstDegree"/>
    /// </summary>
    public class MurderSecondDegree : Murder
    {
        public Predicate<ILegalPerson> IsExtremeIndifferenceToHumanLife { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!IsExtremeIndifferenceToHumanLife(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsExtremeIndifferenceToHumanLife)} is false");
                return false;
            }
            return base.IsValid(persons);
        }
    }
}
