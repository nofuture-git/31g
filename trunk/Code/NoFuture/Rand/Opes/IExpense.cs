using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp;

namespace NoFuture.Rand.Opes
{
    /// <summary>
    /// Expresses a personal expense in time
    /// </summary>
    public interface IExpense : IDeinde, IObviate
    {

        /// <summary>
        /// Returns all current expenses right Now
        /// </summary>
        NamedReceivable[] CurrentExpectedExpenses { get; }

        /// <summary>
        /// Returns all current expenses at the time of <see cref="dt"/>
        /// </summary>
        NamedReceivable[] GetExpectedExpensesAt(DateTime? dt);

        /// <summary>
        /// A monetary sum of all current expenses
        /// </summary>
        Pecuniam TotalAnnualExpectedExpenses { get; }

    }
}
