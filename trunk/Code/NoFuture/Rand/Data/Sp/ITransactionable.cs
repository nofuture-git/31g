using System;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// A general type for duality of financial transactions (e.g. Buy\Sell, Long\Short, CashIn\CashOut)
    /// </summary>
    public interface ITransactionable
    {
        void Push(DateTime dt, Pecuniam amt, Pecuniam fee = null, IMereo note = null);
        bool Pop(DateTime dt, Pecuniam amt, Pecuniam fee = null, IMereo note = null);
    }
}