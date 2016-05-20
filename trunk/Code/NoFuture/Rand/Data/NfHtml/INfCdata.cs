using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Data.NfHtml
{
    public interface INfCdata
    {
        Uri SourceUri { get; }
        List<dynamic> ParseContent(string webResponseBody);
    }
}
