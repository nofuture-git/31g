using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Represents a name of any kind of money entry
    /// </summary>
    public interface IMereo : IVoca
    {
        Interval Interval { get; set; }
        Classification Classification { get; set; }
        string Name { get; set; }
    }
}
