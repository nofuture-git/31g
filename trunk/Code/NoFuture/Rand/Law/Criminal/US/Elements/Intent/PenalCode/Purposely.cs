using System;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.ComLaw;

namespace NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode
{
    /// <summary>
    /// <![CDATA[
    /// intends to bring about harm
    /// see (Model Penal Code § 2.02 (2) (a)).
    /// ]]>
    /// </summary>
    public class Purposely : SpecificIntent
    {
        public override int CompareTo(object obj)
        {
            if (obj is Knowingly || obj is Recklessly || obj is Negligently)
                return 1;
            return 0;
        }
    }
}
