using System;

namespace NoFuture.Rand.Core
{
    [Serializable]
    public class RcharLAlpha : Rchar
    {
        public RcharLAlpha(int indexValue) : base(indexValue) { }

        public override char Rand => (char)Etx.MyRand.Next(0x61, 0x7A);

        public override bool Valid(string dlValue)
        {
            return base.Valid(dlValue) && char.IsLower(dlValue.ToCharArray()[idx]);
        }
    }
}