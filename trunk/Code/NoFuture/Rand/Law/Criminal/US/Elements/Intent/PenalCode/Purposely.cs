using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode
{
    /// <summary>
    /// <![CDATA[
    /// intends to bring about harm
    /// see (Model Penal Code § 2.02 (2) (a)).
    /// ]]>
    /// </summary>
    [Aka("express malice")]
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
