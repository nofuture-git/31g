using System;

namespace NoFuture.Law.Tort.US.Remedy
{
    /// <summary>
    /// Money paid to restore the injured party, to the extent possible, to the
    /// position that would have been occupied had the wrong not occurred
    /// </summary>
    public abstract class CompensatoryDmg : MoneyDmgBase
    {
        protected CompensatoryDmg(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }
    }
}
