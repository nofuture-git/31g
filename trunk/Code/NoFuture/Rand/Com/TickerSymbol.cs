using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Com
{
    [Serializable]
    public class TickerSymbol : Identifier
    {
        private string _symbol;

        public string Symbol
        {
            get
            {
                if (_symbol.Contains(":"))
                    _symbol = _symbol.Split(':')[0];
                return _symbol;
            }
            set
            {
                _symbol = value;
            }
        }

        public string Exchange { get; set; }
        public string InstrumentType { get; set; }
        public string Country { get; set; }
        public override bool Equals(object obj)
        {
            var tkr = obj as TickerSymbol;
            return tkr != null && Equals(tkr);
        }

        public override string Value { get{ return Symbol;} set { Symbol = value; } }

        public override int GetHashCode()
        {
            return (Symbol?.GetHashCode() ?? 0) + (Exchange?.GetHashCode() ?? 0);
        }

        public bool Equals(TickerSymbol obj)
        {
            return string.Equals(Symbol, obj.Symbol, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Exchange, obj.Exchange, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Country, obj.Country, StringComparison.OrdinalIgnoreCase);
        }

        public override string Abbrev => "Ticker";
    }
}