using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <summary>
    /// Attempting to make physical contact but does not 
    /// </summary>
    [Aka("attempted battery", "threatened battery")]
    public class Assault : CriminalBase, IActusReus
    {
        /// <summary>
        /// the ability to cause harful or offensive physical contact
        /// </summary>
        public SubstantialSteps PresentAbility { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;
            var isssub = PresentAbility?.IsValid(persons) ?? false;
            AddReasonEntryRange(PresentAbility?.GetReasonEntries());

            return isssub;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
