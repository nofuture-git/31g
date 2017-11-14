using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Represents single item household income in any form.
    /// </summary>
    public interface IMereo : IVoca
    {
        IncomeInterval Interval { get; set; }
        string Name { get; set; }
    }
}
