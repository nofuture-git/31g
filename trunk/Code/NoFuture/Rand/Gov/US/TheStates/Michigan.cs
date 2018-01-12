using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Michigan : UsState
    {
        public Michigan() : base("MI")
        {
            var dl = new Rchar[13];
            dl[0] = new RcharUAlpha(0);
            dl[1] = new RcharLimited(1, '1', '2', '3', '4', '5', '6');
            dl[2] = new RcharLimited(1, '1', '2', '3', '4', '5', '6');
            dl[3] = new RcharLimited(1, '1', '2', '3', '4', '5', '6');
            Array.Copy(Numerics(9, 4), 0, dl, 4, 9);

            dlFormats = new[] { new DriversLicense(dl, this) };
        }
    }
}