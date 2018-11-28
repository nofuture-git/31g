using System.Collections;
using System.Collections.Generic;

namespace NoFuture.Rand.Sp
{
    public class AccountComparer<T> : IComparer<IAccount<T>>
    {
        private readonly CaseInsensitiveComparer _caseiComp = new CaseInsensitiveComparer();

        public int Compare(IAccount<T> x, IAccount<T> y)
        {
            var xid = x?.Id?.ToString() ?? "";
            var yid = y?.Id?.ToString() ?? "";

            return _caseiComp.Compare(xid, yid);
        }
    }
}