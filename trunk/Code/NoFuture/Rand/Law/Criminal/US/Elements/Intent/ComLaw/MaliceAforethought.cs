using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw
{
    /// <summary>
    /// intent designated only for murder
    /// </summary>
    [Aka("intent to kill")]
    public class MaliceAforethought : GeneralIntent
    {
        public override int CompareTo(object obj)
        {
            if (obj is SpecificIntent || obj is GeneralIntent)
                return 1;
            return 0;
        }
    }
}
