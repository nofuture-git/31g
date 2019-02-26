using System;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <summary>
    /// <![CDATA[ Threat of unlawful force with ability to do it. ]]>
    /// </summary>
    public interface IAssault : ILegalConcept
    {
        Predicate<ILegalPerson> IsByThreatOfForce { get; set; }
    }
}
