using System;

namespace NoFuture.Rand.Law.US.Criminal.Elements.Intent.ComLaw
{
    /// <summary>
    /// without the additional desire to cause a result
    /// </summary>
    public class GeneralIntent : MensRea, IComparable
    {
        public virtual int CompareTo(object obj)
        {
            if (obj is MaliceAforethought || obj is SpecificIntent)
                return -1;
            return 0;
        }
    }
}
