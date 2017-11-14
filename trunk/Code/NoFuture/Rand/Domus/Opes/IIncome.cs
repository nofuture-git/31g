using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    /// <summary>
    /// Represents single item household income in any form.
    /// </summary>
    public interface IIncome : IIdentifier<Pecuniam>, IVoca
    {
        IncomeInterval Interval { get; set; }
        string Name { get; set; }
    }
}
