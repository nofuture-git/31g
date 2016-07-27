using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Shared;
using NoFuture.Util;
using NoFuture.Util.Etymological;
using NoFuture.Util.Etymological.Biz;

namespace NoFuture.Gen
{
    /// <summary>
    /// Utility methods ported from powershell scripts.
    /// </summary>
    public class Matching
    {
        private static readonly string[] stopWords = new[]
        {
            "has", "are", "as", "at", "by", "can", "for", "from", "have", "in", "of", "than", "then", "to", "was",
            "with", "use"
        };

        private static readonly List<INomenclature> nomenclatures = new List<INomenclature>
        {
            new Demonyms(),
            new Metrix(),
            new NetworkResource(),
            new TelecoResource(),
            new Toponyms(),
            new Chronos(),
            new Monetary(),
            new CardinalOrdinal(),
            new Identity(),
            new BizStrings()
        };
    }
}
