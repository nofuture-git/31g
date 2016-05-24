using System;
using System.Collections.Generic;
using System.Linq;

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

    public class TickerComparer : IComparer<Ticker>
    {
        private readonly string _companyName;
        public TickerComparer(string companyName)
        {
            _companyName = companyName;
        }

        protected char FirstCharOfName
        {
            get
            {
                return string.IsNullOrWhiteSpace(_companyName)
                    ? '\0'
                    : _companyName.Trim().ToUpper().ToCharArray().First();
            }
        }

        public int Compare(Ticker x, Ticker y)
        {
            if (x == null && y == null)
                return 0;
            if (y == null || string.IsNullOrWhiteSpace(y.Value))
                return -1;
            if (x == null || string.IsNullOrWhiteSpace(x.Value))
                return 1;

            var p0x = x.Value.Trim().ToUpper().ToCharArray().First() == FirstCharOfName;
            var p0y = y.Value.Trim().ToUpper().ToCharArray().First() == FirstCharOfName;

            var p1x = x.Value.Trim().Length <= 3;
            var p1y = y.Value.Trim().Length <= 3;

            var p2x = x.Value.Trim().Length <= 4;
            var p2y = y.Value.Trim().Length <= 4;

            if (p0x && !p0y)
                return -1;
            if (!p0x && p0y)
                return 1;

            if (p1x && !p1y)
                return -1;
            if (!p1x && p1y)
                return 1;

            if (p2x && !p2y)
                return -1;

            if (!p2x && p2y)
                return 1;

            return x.Value.Trim().Length < y.Value.Trim().Length ? -1 : 1;
        }
    }
}