using System;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <summary>
    /// <![CDATA[
    /// "Force" being physical acts [...] intentionally used [..] to crime
    /// Threat of unlawful force with ability to do it.
    /// ]]>
    /// </summary>
    public interface IAssault
    {
        Predicate<ILegalPerson> IsByThreatOfForce { get; set; }

        /// <summary>
        /// the ability to cause harmful or offensive physical 
        /// contact, even though the contact never takes place
        /// </summary>
        Predicate<ILegalPerson> IsPresentAbility { get; set; }
    }
}
