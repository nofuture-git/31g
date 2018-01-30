using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Sorts by <see cref="ITransaction.AtTime"/>
    /// </summary>
    [Serializable]
    public class TransactionComparer : IComparer<ITransaction>
    {
        public int Compare(ITransaction x, ITransaction y)
        {
            if (x == null)
                return 1;
            if (y == null)
                return -1;
            return DateTime.Compare(x.AtTime, y.AtTime);
        }
    }
}