using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// A defense against Attempt which was motivated by goodness
    /// </summary>
    public interface IAbandonment : ILegalConcept
    {
        Predicate<ILegalPerson> IsMotivatedByFearOfGettingCaught { get; set; }

        Predicate<ILegalPerson> IsMotivatedByNewDifficulty { get; set; }
    }
}