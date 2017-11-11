using System;

namespace NoFuture.Rand.Core
{
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