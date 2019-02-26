using System;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <summary>
    /// <![CDATA[ "Force" being physical acts [...] intentionally used [..] to crime ]]>
    /// </summary>
    public interface IDominionOfForce : ILegalConcept
    {
        /// <summary>
        /// the ability to cause harmful or offensive physical 
        /// contact, even though the contact never takes place
        /// </summary>
        Predicate<ILegalPerson> IsPresentAbility { get; set; }

        /// <summary>
        /// The threat appears real to the victim even if the defendant knows otherwise.
        /// </summary>
        Predicate<ILegalPerson> IsApparentAbility { get; set; }
    }
}
