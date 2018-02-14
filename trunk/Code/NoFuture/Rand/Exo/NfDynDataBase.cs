using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Exo
{
    [Serializable]
    public abstract class NfDynDataBase: INfDynData
    {
        protected NfDynDataBase(Uri src)
        {
            SourceUri = src;
        }
        public Uri SourceUri { get; }
        public abstract IEnumerable<dynamic> ParseContent(object content);
    }
}
