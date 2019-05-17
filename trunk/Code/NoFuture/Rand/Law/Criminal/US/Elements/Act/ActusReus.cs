using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Act
{
    /// <summary>
    /// To be a crime there must have been an action which was voluntary
    /// </summary>
    /// <inheritdoc cref="IActusReus"/>
    [Aka("criminal act")]
    public class ActusReus : Law.US.Act, IActusReus, IElement
    {
        public virtual bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
