using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.Criminal.US
{
    public interface IProsecution : IRationale
    {
        ILegalPerson GetDefendant(params ILegalPerson[] persons);

        Predicate<ILegalPerson> IsVictim { get; set; }
    }
}
