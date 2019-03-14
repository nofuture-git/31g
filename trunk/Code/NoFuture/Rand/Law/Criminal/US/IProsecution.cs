using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.Criminal.US
{
    public interface IProsecution : IRationale
    {
        Func<ILegalPerson[], ILegalPerson> GetDefendant { get; set; }

        Predicate<ILegalPerson> IsVictim { get; set; }
    }
}
