using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus.Opes;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Represents single item household income in any form.
    /// </summary>
    public interface IMereo : IIdentifier<Pecuniam>, IVoca
    {
        IncomeInterval Interval { get; set; }
        string Name { get; set; }
    }
}
