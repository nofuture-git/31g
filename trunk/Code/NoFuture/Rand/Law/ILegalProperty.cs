using NoFuture.Rand.Core;

namespace NoFuture.Rand.Law
{
    /// <summary>
    /// General interface to represent the concept of property (real, personal or otherwise)
    /// </summary>
    public interface ILegalProperty : IVoca, IRationale
    {
        /// <summary>
        /// Who is lawfully entitled to the property
        /// </summary>
        ILegalPerson EntitledTo { get; set; }

        /// <summary>
        /// Who actually possess control over the property
        /// </summary>
        ILegalPerson InPossessionOf { get; set; }
    }
}
