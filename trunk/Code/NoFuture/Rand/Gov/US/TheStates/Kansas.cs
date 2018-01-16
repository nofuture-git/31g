using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Kansas : UsState
    {
        public Kansas() : base("KS")
        {
            var dl = new Rchar[9];
            dl[0] = new RcharUAlpha(0);
            Array.Copy(Numerics(8, 1), 0, dl, 1, 8);

            dlFormats = new[] { new DriversLicense(dl, this) };
        }
    }
}