

using System;

namespace NoFuture.Rand.Law.US.Contracts.Remedy
{
    /// <inheritdoc />
    /// <summary>
    /// Upon a contract with no defenses, terms are understood, there is 
    /// a breach and its unexcused - what should the plaintiff get.
    /// </summary>
    /// <remarks>
    /// are awarded only to correct a private wrong, no to vindicate a public interest - make the 
    /// non-breaching party whole.
    /// </remarks>
    public abstract class RemedyBase<T> : DilemmaBase<T> where T : IObjectiveLegalConcept
    {
        protected RemedyBase(IContract<T> contract) : base(contract)
        {
        }
        
    }
}
