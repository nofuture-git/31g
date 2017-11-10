using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Data.Exo
{
    public interface INfDynData
    {
        Uri SourceUri { get; }
        IEnumerable<dynamic> ParseContent(object content);
    }
}
