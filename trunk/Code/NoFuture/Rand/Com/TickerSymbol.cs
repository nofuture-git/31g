using System;

namespace NoFuture.Rand.Com
{
    [Serializable]
    public class Ticker
    {
        public string Symbol { get; set; }
        public string Exchange { get; set; }
        public string InstrumentType { get; set; }

        public bool Equals(Ticker obj)
        {
            return string.Equals(Symbol, obj.Symbol, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Exchange, obj.Exchange, StringComparison.OrdinalIgnoreCase);
        }
    }
}