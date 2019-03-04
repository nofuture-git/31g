using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Criminal.US.Elements
{
    /// <summary>
    /// <![CDATA[ Threat of unlawful force with ability to do it. ]]>
    /// </summary>
    public interface IAssault : IElement
    {
        [Aka("constructive force")]
        Predicate<ILegalPerson> IsByThreatOfForce { get; set; }
    }
}
