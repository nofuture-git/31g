using System;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <summary>
    /// <![CDATA[ "Force" being physical acts [...] intentionally used [..] to crime ]]>
    /// </summary>
    public interface IDominionOfForce
    {
        /// <summary>
        /// the ability to cause harmful or offensive physical 
        /// contact, even though the contact never takes place
        /// </summary>
        Predicate<ILegalPerson> IsPresentAbility { get; set; }
    }
}
