using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// <![CDATA[
    /// threat of force or other unlawful action to induce to consent
    /// ]]>
    /// </summary>
    [Aka("offer (s)he couldn't refuse")]
    public interface IDuress : ILegalConcept
    {
        Predicate<ILegalPerson> IsAssentByPhysicalCompulsion { get; set; }

        IImproperThreat ImproperThreat { get; set; }
    }
}