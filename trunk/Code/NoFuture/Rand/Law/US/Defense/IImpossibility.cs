using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// A defense to Attempt
    /// </summary>
    public interface IImpossibility : ILegalConcept
    {
        /// <summary>
        /// the defendant believes what they are doing is illegal but its not
        /// </summary>
        Predicate<ILegalPerson> IsLegalImpossibility { get; set; }

        /// <summary>
        /// the crime failed because the facts are not as he or she believes them to be
        /// </summary>
        Predicate<ILegalPerson> IsFactualImpossibility { get; set; }
    }
}