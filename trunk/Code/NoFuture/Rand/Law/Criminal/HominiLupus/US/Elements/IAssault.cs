using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <summary>
    /// <![CDATA[ Threat of unlawful force with ability to do it. ]]>
    /// </summary>
    public interface IAssault : IActusReus
    {
        Predicate<ILegalPerson> IsByThreatOfForce { get; set; }
    }
}
