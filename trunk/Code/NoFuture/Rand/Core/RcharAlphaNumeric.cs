using System;

namespace NoFuture.Rand.Core
{
    [Serializable]
    public class RcharAlphaNumeric : Rchar
    {
        public RcharAlphaNumeric(int indexValue) : base(indexValue) { }

        public override char Rand
        {
            get
            {
                var pickOne = Etx.CoinToss();
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
}