using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Data.Types
{
    public interface INfDynData
    {
        Uri SourceUri { get; }
        List<dynamic> ParseContent(object content);
    }
}
