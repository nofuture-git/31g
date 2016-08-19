using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Data.Types
{
    public abstract class NfDynDataBase: INfDynData
    {
        protected NfDynDataBase(Uri src)
        {
            SourceUri = src;
        }
        public Uri SourceUri { get; }
        public abstract List<dynamic> ParseContent(object content);
    }
}
