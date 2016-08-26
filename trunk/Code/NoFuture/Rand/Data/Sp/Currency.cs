using System;

namespace NoFuture.Rand.Data.Sp
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
            get
            {
                return Abbrev;
            }
            set
            {
                if (!Enum.TryParse(value, true, out _abbrev))
                    throw new NotImplementedException($"The currency type {value} is unknown.");
            }
        }
    }
}
