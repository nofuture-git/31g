using System;

namespace NoFuture.Rand.Data.Types
{
    [Serializable]
    internal class YahooTickerResultSet
    {
        internal YahooTickerResult ResultSet;
    }

    [Serializable]
    internal class YahooTickerResult
    {
        internal string Query;
        internal YahooTickerSymbol[] Result;
    }

    [Serializable]
    internal class YahooTickerSymbol
    {
        internal string symbol;
        internal string name;
        internal string exch;
        internal string type;
        internal string exchDisp;
        internal string typeDisp;
    }
}
