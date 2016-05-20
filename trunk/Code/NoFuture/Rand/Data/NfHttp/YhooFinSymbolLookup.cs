using System;
using System.Collections.Generic;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Data.NfHttp
{
    public class YhooFinSymbolLookup : INfDynData
    {
        private readonly Uri _srcUri;
        public YhooFinSymbolLookup(Uri srcUri)
        {
            _srcUri = srcUri;
        }

        public Uri SourceUri { get {return _srcUri;} }
        public List<dynamic> ParseContent(object content)
        {
            throw new NotImplementedException();
        }
    }
}
