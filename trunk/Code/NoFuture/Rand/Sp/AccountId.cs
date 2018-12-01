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

        protected internal virtual void SetRefId(int? referenceId)
        {
            _refId = referenceId;
        }

        protected internal virtual void SetRefId(string referenceId)
        {
            if (string.IsNullOrWhiteSpace(referenceId))
                _refId = null;
            if (int.TryParse(referenceId, out var valInt))
                _refId = valInt;
        }

        /// <summary>
        /// Will return the refId value given in the ctor, if it was present
        /// </summary>
        public override string Abbrev => _refId?.ToString() ?? "Acct";

        /// <summary>
        /// Helper method to determine if a name matches this account id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool Equals(IVoca name)
        {
            return name != null && name.AnyNames(n => n == Value);
        }
    }
}