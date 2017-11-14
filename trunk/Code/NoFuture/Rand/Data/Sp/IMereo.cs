﻿using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Represents a name of any kind of money entry
    /// </summary>
    public interface IMereo : IVoca
    {
        IncomeInterval Interval { get; set; }
        string Name { get; set; }
    }
}
