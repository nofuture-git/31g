using System;
using System.Linq;

namespace NoFuture.Rand.Core
{
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
}