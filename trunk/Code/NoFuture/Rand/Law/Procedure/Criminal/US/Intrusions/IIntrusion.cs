﻿using System;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Intrusions
{
    public interface IIntrusion : ILegalConcept
    {
        Func<ILegalPerson[], ILegalPerson> GetSuspect { get; set; }

        Func<ILegalPerson[], ILegalPerson> GetLawEnforcement { get; set; }
    }
}
