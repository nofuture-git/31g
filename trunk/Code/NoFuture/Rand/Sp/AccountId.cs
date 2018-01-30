using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    [Serializable]
    public class AccountId : RIdentifier
    {
        public AccountId(Rchar[] format)
        {
            this.format = format;
        }

        public override string Abbrev => "Acct";
    }
}