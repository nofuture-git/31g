using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;

namespace NoFuture.Rand.Law.Criminal.Homicide.US.Elements
{
    public class MurderFirstDegree : Murder, ICapitalOffense
    {
        /// <summary>
        /// purposeful, planned, calculated, designed
        /// </summary>
        public Predicate<ILegalPerson> IsPremediated { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsPremediated(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsPremediated)} is false");
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
