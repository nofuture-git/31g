using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Rand.Data.Types
{
    public interface INfDynData
    {
        Uri SourceUri { get; }
        List<dynamic> ParseContent(object content);
    }
}
