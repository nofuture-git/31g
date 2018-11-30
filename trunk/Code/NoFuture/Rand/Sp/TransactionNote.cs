using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    /// <summary>
    /// A general purpose extension of the Rand.IVoca (i.e. Name)
    /// </summary>
    [Serializable]
    public class TransactionNote : VocaBase
    {
        private string _additionalInfo;
        public TransactionNote(){ }
        public TransactionNote(string accountName) :base(accountName) { }

        public TransactionNote(string accountName, string additionalInfo) : base(accountName)
        {
            _additionalInfo = additionalInfo;
        }

        public virtual string AdditionalInformation
        {
            get => _additionalInfo;
            set => _additionalInfo = value;
        }
    }
}
