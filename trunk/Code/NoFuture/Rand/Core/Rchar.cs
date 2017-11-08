using System;
using System.Linq;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// A type for defining a character which is 
    /// randomized, but also constrained by some kind of rule.
    /// A string of these creates a highly controlled randomization pattern.
    /// </summary>
    [Serializable]
    public abstract class Rchar
    {
        protected readonly int idx;
        public abstract char Rand { get; }

        public virtual bool Valid(string dlValue)
        {
            if (string.IsNullOrWhiteSpace(dlValue))
                return false;
            return dlValue.Length - 1 >= idx;
        }
        protected Rchar(int indexValue)
        {
            idx = indexValue;
        }
    }

    [Serializable]
    public class RcharAlphaNumeric : Rchar
    {
        public RcharAlphaNumeric(int indexValue) : base(indexValue) { }

        public override char Rand
        {
            get
            {
                var pickOne = Etx.CoinToss;
                if(pickOne)
                    return (char) Etx.MyRand.Next(0x41, 0x5A);

                return (char)Etx.MyRand.Next(0x30, 0x39);
            }
        }
        public override bool Valid(string dlValue)
        {
            var assertion = char.IsUpper(dlValue.ToCharArray()[idx]) || char.IsNumber(dlValue.ToCharArray()[idx]);
            return base.Valid(dlValue) && assertion;
        }
    }

    [Serializable]
    public class RcharUAlpha : Rchar
    {
        public RcharUAlpha(int indexValue) : base(indexValue) { }

        public override char Rand => (char) Etx.MyRand.Next(0x41, 0x5A);

        public override bool Valid(string dlValue)
        {
            return base.Valid(dlValue) && char.IsUpper(dlValue.ToCharArray()[idx]);
        }
    }

    [Serializable]
    public class LAlphaRchar : Rchar
    {
        public LAlphaRchar(int indexValue) : base(indexValue) { }

        public override char Rand => (char)Etx.MyRand.Next(0x61, 0x7A);

        public override bool Valid(string dlValue)
        {
            return base.Valid(dlValue) && char.IsLower(dlValue.ToCharArray()[idx]);
        }
    }

    [Serializable]
    public class RcharLimited : Rchar
    {
        private readonly char[] _limitedTo;

        public RcharLimited(int indexValue, params char[] limitedTo) : base(indexValue)
        {
            _limitedTo = limitedTo;
        }

        public override char Rand
        {
            get
            {
                if (_limitedTo == null || _limitedTo.Length <= 0)
                    return 'A';//whatever
                if (_limitedTo.Length == 1)
                    return _limitedTo[0];//easy exit

                var pickone = Etx.MyRand.Next(0, _limitedTo.Length-1);
                return _limitedTo[pickone];
            }
        }
        public override bool Valid(string dlValue)
        {
            if (!base.Valid(dlValue))
                return false;
            var tChar = dlValue.ToCharArray()[idx];
            return _limitedTo.Any(lc => lc == tChar);
        }        
    }

    [Serializable]
    public class RcharNumeric : Rchar
    {
        public RcharNumeric(int indexValue) : base(indexValue) { }
        public override char Rand => (char)Etx.MyRand.Next(0x30, 0x39);

        public override bool Valid(string dlValue)
        {
            return base.Valid(dlValue) && char.IsNumber(dlValue.ToCharArray()[idx]);
        }

    }
}
