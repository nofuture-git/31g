using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Data.Exo
{
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
