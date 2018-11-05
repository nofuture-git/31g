using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    [Serializable]
    public class AccountId : RIdentifier
    {
        public AccountId(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));
            format = DeriveFromValue(id);
            _value = id;
        }

        public AccountId(Rchar[] format)
        {
            this.format = format;
        }

        public override string Abbrev => "Acct";
    }
}