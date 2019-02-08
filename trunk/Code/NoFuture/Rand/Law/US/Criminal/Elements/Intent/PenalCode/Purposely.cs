using System;

namespace NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode
{
    /// <summary>
    /// <![CDATA[
    /// intends to bring about harm
    /// see (Model Penal Code § 2.02 (2) (a)).
    /// ]]>
    /// </summary>
    public class Purposely : MensRea, IComparable
    {
        public virtual int CompareTo(object obj)
        {
            if (obj is Knowingly || obj is Recklessly || obj is Negligently)
                return 1;
            return 0;
        }
    }
}
