using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    [Serializable]
    public class AccountId : RIdentifier
    {
        private int? _refId;
        public AccountId(string id, int? refId):this(id)
        {
            _refId = refId;
        }

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

        protected internal virtual void SetRefId(int? id)
        {
            _refId = id;
        }
        public override string Abbrev => _refId?.ToString() ?? "Acct";
    }
}