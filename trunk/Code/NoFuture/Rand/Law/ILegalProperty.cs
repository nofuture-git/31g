using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law
{
    /// <summary>
    /// General interface to represent the concept of property (real, personal or otherwise)
    /// </summary>
    /// <remarks>
    /// is entirely the creature of law ... is only a(n) expectation
    /// </remarks>
    public interface ILegalProperty : IVoca, IRationale, IRankable
    {
        /// <summary>
        /// Who is lawfully entitled to the property
        /// </summary>
        ILegalPerson EntitledTo { get; set; }

        /// <summary>
        /// Who actually possess control over the property - more than just physical possession.
        /// </summary>
        /// <remarks>
        /// Meaning of possession differs by context where it ranges between communicated intent
        /// to a physical act.  However, once established (by whatever means) it becomes a legally protected interest.
        /// </remarks>
        [Aka("seisin")]
        ILegalPerson InPossessionOf { get; set; }

        decimal? PropertyValue { get; set; }
    }
}
