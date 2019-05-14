using System;

namespace NoFuture.Rand.Law.US.Defense
{
    public interface IMentalIncompetent : ILegalConcept
    {
        /// <summary>
        /// A person considered by the court to be without the capacity to contract
        /// </summary>
        Predicate<ILegalPerson> IsMentallyIncompetent { get; set; }
    }
}