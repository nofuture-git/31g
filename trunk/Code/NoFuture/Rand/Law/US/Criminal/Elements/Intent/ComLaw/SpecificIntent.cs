
namespace NoFuture.Rand.Law.US.Criminal.Elements.Intent.ComLaw
{
    /// <summary>
    /// A more sophisticated level of awareness
    /// </summary>
    public class SpecificIntent : GeneralIntent
    {
        public override int CompareTo(object obj)
        {
            if (obj is MaliceAforethought)
                return -1;
            if (obj is GeneralIntent)
                return 1;
            return 0;
        }
    }
}
