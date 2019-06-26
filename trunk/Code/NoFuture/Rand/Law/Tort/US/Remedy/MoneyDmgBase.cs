using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Remedy
{
    /// <summary>
    /// The only form of remedy available in the United States.
    /// Other forms of remedy are public apologies, acts of service and jail time.
    /// </summary>
    public abstract class MoneyDmgBase : UnoHomine
    {
        protected MoneyDmgBase(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }
    }
}
