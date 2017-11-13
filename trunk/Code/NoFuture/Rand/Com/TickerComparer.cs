using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// For sorting <see cref="TickerSymbol"/>
    /// </summary>
    [Serializable]
    public class TickerComparer : IComparer<TickerSymbol>
    {
        private readonly string _companyName;
        private readonly Regex _companyNameRegex;
        public TickerComparer(string companyName)
        {
            if(string.IsNullOrWhiteSpace(companyName))
                throw new ArgumentNullException(nameof(companyName));

            _companyName = companyName;
            _companyNameRegex = new Regex(RegexCatalog.ToRegexExpression(_companyName), RegexOptions.IgnoreCase);
        }

        protected char FirstCharOfName => string.IsNullOrWhiteSpace(_companyName)
            ? '\0'
            : _companyName.Trim().ToUpper().ToCharArray().First();

        public int Compare(TickerSymbol x, TickerSymbol y)
        {
            const string USA = "USA";
            if (x == null && y == null)
                return 0;
            if (string.IsNullOrWhiteSpace(y?.Value))
                return -1;
            if (string.IsNullOrWhiteSpace(x?.Value))
                return 1;

            Func<TickerSymbol, bool[]> getProps = ticker => new[]
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