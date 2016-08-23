using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Data.Types
{
    public interface INfDynData
    {
        Uri SourceUri { get; }
        IEnumerable<dynamic> ParseContent(object content);
    }
}
