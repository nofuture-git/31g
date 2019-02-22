using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.Homicide.US.Elements
{
    /// <summary>
    /// This is the same as <see cref="Murder"/> only the intent must be <see cref="AdequateProvocation"/>
    /// </summary>
    public class ManslaughterVoluntary : Manslaughter
    {
        public override bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var adequateProvation = criminalIntent as AdequateProvocation;
            var defendant = GetDefendant(persons);
            if (adequateProvation != null && defendant != null && adequateProvation.IsValid(defendant))
            {
                return true;
            }
            AddReasonEntry($"{nameof(ManslaughterVoluntary)} is a redux of {nameof(Murder)} " +
                           $"by means of {nameof(AdequateProvocation)} intent, " +
                           $"which {criminalIntent?.GetType().Name} is not");

            return false;
        }
    }
}
