using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <inheritdoc cref="IBipartite"/>
    /// <summary>
    /// This seems to imply that the parties know they have some family relation
    /// </summary>
    public class Incest : CriminalBase, IActusReus, IBipartite
    {
        public Predicate<ILegalPerson> IsSexualIntercourse { get; set; } = lp => false;

        public Func<ILegalPerson[], ILegalPerson> GetVictim { get; set; } = lps => null;

        /// <summary>
        /// Typically being any family member one could not marry
        /// </summary>
        public Func<ILegalPerson, ILegalPerson, bool> IsFamilyRelation { get; set; } = (lp1, lp2) => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var victim = GetVictim(persons);
            if (victim == null)
            {
                AddReasonEntry($"the {nameof(GetVictim)} returned null");
                return false;
            }

            if (!IsSexualIntercourse(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsSexualIntercourse)} is false");
                return false;
            }

            var isFamily = IsFamilyRelation(defendant, victim);

            AddReasonEntry($"{nameof(IsFamilyRelation)} is {isFamily} for {defendant.Name} and {victim.Name}");
            return isFamily;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return criminalIntent != null;
        }
    }
}
