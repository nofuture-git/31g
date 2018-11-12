using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public abstract class GovernmentId : RIdentifier, IObviate
    {
        public DateTime? IssuedDate { get; set; }
        public virtual IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>();
            var v = Value;
            if (!string.IsNullOrWhiteSpace(v))
                itemData.Add(textFormat(GetType().Name), Value);
            return itemData;
        }
    }
}