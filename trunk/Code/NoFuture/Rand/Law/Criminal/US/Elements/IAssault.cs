using System;
using NoFuture.Rand.Law.Criminal.US.Elements;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US
{
    /// <summary>
    /// <![CDATA[ Threat of unlawful force with ability to do it. ]]>
    /// </summary>
    public interface IAssault : IElement
    {
        Predicate<ILegalPerson> IsByThreatOfForce { get; set; }
    }
}
