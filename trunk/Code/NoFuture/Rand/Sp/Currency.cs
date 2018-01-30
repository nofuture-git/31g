using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    [Serializable]
    public sealed class Currency : Identifier
    {
        private CurrencyAbbrev _abbrev;
        public Currency(CurrencyAbbrev v)
        {
            _abbrev = v;
        }
        public override string Abbrev => Enum.GetName(typeof(CurrencyAbbrev), _abbrev);

        public override string Value
        {
            get => Abbrev;
            set
            {
                if (!Enum.TryParse(value, true, out _abbrev))
                    throw new NotImplementedException($"The currency type {value} is unknown.");
            }
        }
    }
}
