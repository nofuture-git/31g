﻿using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Criminal
{
    /// <summary>
    /// Threat of unlawful violent force with ability to do it.
    /// </summary>
    public interface IAssault : ILegalConcept
    {
        [Aka("constructive force")]
        Predicate<ILegalPerson> IsByThreatOfViolence { get; set; }
    }
}
