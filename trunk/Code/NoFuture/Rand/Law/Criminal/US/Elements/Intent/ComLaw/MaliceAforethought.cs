using NoFuture.Law.Attributes;

namespace NoFuture.Law.Criminal.US.Elements.Intent.ComLaw
{
    /// <summary>
    /// intent designated only for murder - means premeditated
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
