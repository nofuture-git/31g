using System;

namespace NoFuture.Rand.Com
{
    [Serializable]
    public class Ticker : Identifier
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

        public override bool Equals(object obj)
        {
            var tkr = obj as Ticker;
            if (tkr == null)
                return false;
            return Equals(tkr);
        }

        public override string Value { get{ return Symbol;} set { Symbol = value; } }

        public override int GetHashCode()
        {
            return (Symbol == null ? 0 : Symbol.GetHashCode()) + (Exchange == null ? 0 : Exchange.GetHashCode());
        }

        public bool Equals(Ticker obj)
        {
            return string.Equals(Symbol, obj.Symbol, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Exchange, obj.Exchange, StringComparison.OrdinalIgnoreCase);
        }

        public override string Abbrev
        {
            get { return "Ticker"; }
        }
    }
}