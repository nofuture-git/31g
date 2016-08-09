using System;

namespace NoFuture.Rand.Data.Sp
{
    public interface ITransactionable
    {
        void PutCashIn(DateTime dt, Pecuniam amt, string note = null);
        bool TakeCashOut(DateTime dt, Pecuniam amt, string note = null);
    }
}
