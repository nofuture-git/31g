using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// Another kind of security identifier used is stock exchanges around the world.
    /// </summary>
    [Serializable]
    public class TickerSymbol : Identifier
    {
        private string _symbol;
        private static List<NasdaqIntegratedSymbology> _symbolXref = new List<NasdaqIntegratedSymbology>();

        public string Symbol
        {
            get
            {
                if (_symbol.Contains(":"))
                    _symbol = _symbol.Split(':')[0];
                return _symbol;
            }
            set => _symbol = value;
        }

        public string UriEncodedSymbol => Util.Core.Etc.EscapeString(Symbol, EscapeStringType.URI);

        public string Exchange { get; set; }
        public string InstrumentType { get; set; }
        public string Country { get; set; }
        public override bool Equals(object obj)
        {
            var tkr = obj as TickerSymbol;
            return tkr != null && Equals(tkr);
        }

        public override string Value
        {
            get => Symbol;
            set => Symbol = value;
        }

        /// <summary>
        /// See [http://www.nasdaqtrader.com/trader.aspx?id=CQSsymbolconvention]
        /// </summary>
        public static IEnumerable<NasdaqIntegratedSymbology> TickerSymbolSuffixes
        {
            get
            {
                if (_symbolXref.Any())
                    return _symbolXref;
                _symbolXref.Add(new NasdaqIntegratedSymbology("Preferred", "p", "PR", "-", "$"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Preferred Class \"A\"*", "pA", "PRA", "-A", "�$A"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Preferred Class \"B\"*", "pB", "PRB", "-B", "$B"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Class \"A\"*", "/A", "A", ".A", "�.A"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Class \"B\"*", "/B", "B", ".B", "� .B"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Preferred when distributed", "p/WD", "PRWD", "-$", ".D"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("When distributed", "/WD", "WD", "$", ".Z"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Warrants", "/WS", "WS", "+", ".W"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Warrants Class \"A\"*", "/WS/A", "WSA", "+A", ".W or .A**"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Warrants Class \"B\"*", "/WS/B", "WSB", "+B", ""));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Called", "/CL", "CL", "*", ""));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Class \"A\" Called*", "/A/CL", "ACL", ".A*", ".A"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Preferred called", "p/CL", "PRCL", "-*", "$"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Preferred \"A\" called*", "pA/CL", "PRACL", "-A*", "$A"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Preferred \"A\" when issued*", "pAw", "PRAWI", "-A#", "�.V or .Z"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Emerging Company Marketplace", "/EC", "EC", "!", ".E"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Partial Paid", "/PP", "PP", "@", ""));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Convertible", "/CV", "CV", "%", ""));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Convertible called", "/CV/CL", "CVCL", "%*", ""));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Class Convertible", "/A/CV", "ACV", ".A%", ""));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Preferred (class A) Convertible", "pA/CV", "PRACV", "-A%", ""));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Preferred (class A) when Distributed", "pA/WD", "PRAWD", "-A$", ""));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Rights", "r", "RT", "^", ".R"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Units", "/U", "U", "=", ".U"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("When issued", "w", "WI", "#", ".V or .Z"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Rights when issued", "rw", "RTWI", "^#", ".V or .Z"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Preferred when issued", "pw", "PRWI", "-#", ".V or .Z"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Class \"A\" when issued*", "/Aw", "AWI", ".A#", ".V or .Z"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("Warrrant when issued", "/WSw", "WSWI", "+#", ".V or .Z"));
                _symbolXref.Add(new NasdaqIntegratedSymbology("TEST symbol", "/TEST", "TEST", "~", ""));

                return _symbolXref;
            }
        }

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

        /// <summary>
        /// A cross reference type between the different subtleties of 
        /// a tickers symbols
        /// </summary>
        public class NasdaqIntegratedSymbology : ICited
        {
            public string Src
            {
                get { return "http://www.nasdaqtrader.com/trader.aspx?id=CQSsymbolconvention"; }
                set
                {
                    //no-op
                }
            }

            public NasdaqIntegratedSymbology(string name, string cqs, string cms, string integrated, string actCtci)
            {
                Name = name;
                CqsSuffix = cqs;
                CmsSuffix = cms;
                NasdaqIntegratedSuffix = integrated;
                ActCtciSuffixes = actCtci;
            }
            public string Name { get; }
            public string CqsSuffix { get; }
            public string CmsSuffix { get; }
            public string NasdaqIntegratedSuffix { get; }
            public string ActCtciSuffixes { get; }

            public override bool Equals(object obj)
            {
                if (!(obj is NasdaqIntegratedSymbology nis))
                    return false;
                return CqsSuffix == nis.CqsSuffix &&
                       CmsSuffix == nis.CmsSuffix &&
                       NasdaqIntegratedSuffix == nis.NasdaqIntegratedSuffix &&
                       ActCtciSuffixes == nis.ActCtciSuffixes;
            }

            public override int GetHashCode()
            {
                return CqsSuffix?.GetHashCode() ?? 1 +
                       CmsSuffix?.GetHashCode() ?? 1 +
                       NasdaqIntegratedSuffix?.GetHashCode() ?? 1 +
                       ActCtciSuffixes?.GetHashCode() ?? 1;
            }

        }
    }
}