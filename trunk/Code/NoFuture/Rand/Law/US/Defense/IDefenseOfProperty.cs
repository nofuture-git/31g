using System;

namespace NoFuture.Rand.Law.US.Defense
{
    public interface IDefenseOfProperty : ILegalConcept
    {
        /// <summary>
        /// This is objective OR subjective based on jurisdiction
        /// </summary>
        Predicate<ILegalPerson> IsBeliefProtectProperty { get; set; }

        /// <summary>
        /// (1) an unprovoked attack
        /// </summary>
        Provocation Provocation { get; set; }

        /// <summary>
        /// (2) an attack which threatens imminent injury or death 
        ///     to a person or or damage, destruction, or theft to real or personal property
        /// </summary>
        Imminence Imminence { get; set; }

        /// <summary>
        /// (3) an objectively reasonable degree of force, used in response
        /// </summary>
        Proportionality<ITermCategory> Proportionality { get; set; }
    }
}