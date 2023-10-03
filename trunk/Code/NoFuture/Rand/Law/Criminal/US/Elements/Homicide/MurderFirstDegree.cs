using System;
using NoFuture.Law.Criminal.US.Elements.Act;
using NoFuture.Law.Criminal.US.Elements.Intent;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.Homicide
{
    public class MurderFirstDegree : Murder, ICapitalOffense
    {
        /// <summary>
        /// purposeful, planned, calculated, designed
        /// </summary>
        public Predicate<ILegalPerson> IsPremediated { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!IsPremediated(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsPremediated)} is false");
                return false;
            }

            return base.IsValid(persons);
        }

        public override bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var intent = (criminalIntent as DeadlyWeapon)?.UtilizedWith ?? criminalIntent;

            var isRequiredIntent = intent is MaliceAforethought
                                   || intent is SpecificIntent;
                
            if (!isRequiredIntent)
            {
                AddReasonEntry("first-degree means rationally, purposefully, taking " +
                               "steps that culminate in victim's death");
                return false;
            }


            return base.CompareTo(criminalIntent, persons);
        }
    }
}
