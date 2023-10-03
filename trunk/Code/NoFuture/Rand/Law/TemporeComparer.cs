using System;
using System.Collections.Generic;

namespace NoFuture.Law
{
    /// <summary>
    /// An implementation to order <see cref="ITempore"/>
    /// </summary>
    [Serializable]
    public class TemporeComparer : IComparer<ITempore>
    {
        public int Compare(ITempore x, ITempore y)
        {
            if (x?.Inception == null && y?.Inception == null)
                return 0;
            if (x?.Inception == null)
                return 1;
            if (y?.Inception == null)
                return -1;
            return DateTime.Compare(x.Inception, y.Inception);
        }
    }
}