using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Exo
{
    /// <summary>
    /// API type used to perform custom parse of exogenous data.
    /// </summary>
    public interface INfDynData
    {
        /// <summary>
        /// The original URI from which the parse content is derived.
        /// </summary>
        Uri SourceUri { get; }

        /// <summary>
        /// A general purpose method to receive data, in any form, and 
        /// parse it into structured data.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        IEnumerable<dynamic> ParseContent(object content);
    }
}
