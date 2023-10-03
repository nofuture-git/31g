using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.Homicide
{
    /// <summary>
    /// This is the same as <see cref="Murder"/> only the intent must be <see cref="AdequateProvocation"/>
    /// </summary>
    public class ManslaughterVoluntary : Manslaughter
    {
        public override bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var adequateProvation = criminalIntent as AdequateProvocation;
            var defendant = this.Defendant(persons);
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
