using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Elements;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <summary>
    /// Attempting to make physical contact but does not 
    /// </summary>
    [Aka("attempted battery")]
    public class AttemptedBatteryAssault : Attempt, IAssault
    {
        public Predicate<ILegalPerson> IsByThreatOfForce { get; set; } = lp => true;

        public Predicate<ILegalPerson> IsPresentAbility { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;
            return base.IsValid(persons) || IsPresentAbility(defendant);

        }
    }
}
