using System;

namespace NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode
{
    /// <summary>
    /// <![CDATA[
    /// see (Model Penal Code § 2.02(2) (d))
    /// ]]>
    /// </summary>
    public class Negligently : MensRea, IComparable
    {
        public virtual int CompareTo(object obj)
        {
            if (obj is Purposely || obj is Knowingly || obj is Recklessly)
                return -1;
            return 0;
        }
    }
}
