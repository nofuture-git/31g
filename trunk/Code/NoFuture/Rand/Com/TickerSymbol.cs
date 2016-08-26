using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NoFuture.Shared;

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
        public string Country { get; set; }
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
            return (Symbol?.GetHashCode() ?? 0) + (Exchange?.GetHashCode() ?? 0);
        }

        public bool Equals(Ticker obj)
        {
            return string.Equals(Symbol, obj.Symbol, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Exchange, obj.Exchange, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Country, obj.Country, StringComparison.OrdinalIgnoreCase);
        }

        public override string Abbrev
        {
            get { return "Ticker"; }
        }
    }

    [Serializable]
    public class TickerComparer : IComparer<Ticker>
    {
        private readonly string _companyName;
        private readonly Regex _companyNameRegex;
        public TickerComparer(string companyName)
        {
            if(string.IsNullOrWhiteSpace(companyName))
                throw new ArgumentNullException("companyName");

            _companyName = companyName;
            _companyNameRegex = new Regex(RegexCatalog.ToRegexExpression(_companyName), RegexOptions.IgnoreCase);
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
            const string USA = "USA";
            if (x == null && y == null)
                return 0;
            if (y == null || string.IsNullOrWhiteSpace(y.Value))
                return -1;
            if (x == null || string.IsNullOrWhiteSpace(x.Value))
                return 1;

            Func<Ticker, bool[]> getProps = ticker => new[]
            {
                _companyNameRegex.IsMatch(ticker.Value.Trim()),
                string.Equals(ticker.Country, USA, StringComparison.OrdinalIgnoreCase),
                ticker.Value.Trim().ToUpper().ToCharArray().First() == FirstCharOfName,
                ticker.Value.Trim().Length <= 3,
                ticker.Value.Trim().Length <= 4
            };

            var propX = getProps(x);

            var propY = getProps(y);

            for (var i = 0; i < propX.Length; i++)
            {
                if (propX[i] && !propY[i])
                    return -1;
                if (!propX[i] && propY[i])
                    return 1;
            }
            return x.Value.Trim().Length < y.Value.Trim().Length ? -1 : 1;
        }
    }
}