using System;
using System.IO;

namespace NoFuture.Hbm.Command.Receivers
{
    [Serializable]
    public abstract class ReceiverBase<T, L> : IReceiver<T, L>
    {
        public abstract bool IsIdAssigned { get; }
        public abstract L DataId { get; }
        public IReceiverStatus Status { get; set; }

        public abstract NHibernate.Criterion.ICriterion CustomSearch();

        public string ToJson()
        {
            var jsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(this.GetType());
            var ms = new MemoryStream();
            jsonSerializer.WriteObject(ms, this);
            var rdr = new StreamReader(ms);
            ms.Position = 0;
            return rdr.ReadToEnd();
        }

        public abstract T Data { get; set; }
    }
}
