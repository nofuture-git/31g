using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode
{
    /// <summary>
    /// <![CDATA[
    /// practically certain harm will result
    /// see (Model Penal Code in § 2.02(2) (b))
    /// ]]>
    /// </summary>
    public class Knowingly : GeneralIntent
    {
        public override int CompareTo(object obj)
        {
            if (obj is Purposely)
                return -1;
            if (obj is Recklessly || obj is Negligently)
                return 1;
            return 0;
        }
    }
}
