using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Criminal.Homicide.US.Elements
{
    /// <summary>
    /// the state will assume any intentional homicide is <see cref="Murder"/>
    /// which is why this extends it and not the other way around
    /// </summary>
    [Aka("mitigated homicide")]
    public abstract class Manslaughter : Murder
    {
        public override bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
