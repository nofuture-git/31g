using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.Criminal.US
{
    public interface IProsecution
    {
        ILegalPerson GetDefendant(params ILegalPerson[] persons);

        Predicate<ILegalPerson> IsVictim { get; set; }
    }
}
