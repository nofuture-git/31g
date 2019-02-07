using System;

namespace NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode
{
    /// <summary>
    /// <![CDATA[
    /// engage in conduct being aware of the nature and a practically certain result
    /// see (Model Penal Code in § 2.02(2) (b))
    /// ]]>
    /// </summary>
    public class Knowingly : MensRea, IComparable
    {
        public virtual int CompareTo(object obj)
        {
            if (obj is Purposely)
                return -1;
            if (obj is Recklessly || obj is Negligently)
                return 1;
            return 0;
        }
    }
}
