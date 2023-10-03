using System;
using NoFuture.Law.Attributes;

namespace NoFuture.Law.Criminal
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
