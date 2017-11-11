using System;

namespace NoFuture.Rand.Core
{
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
}