using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Connecticut : UsState
    {
        public Connecticut() : base("CT")
        {
            var dl = new Rchar[9];
            dl[0] = new RcharLimited(0,'0');
            dl[1] = new RcharLimited(1, '1', '2', '3', '4', '5', '6', '7', '8', '9');
            Array.Copy(Numerics(7, 2), 0, dl, 2, 7);
            var dlf00 = new DriversLicense(dl, this);

            dl = new Rchar[9];
            dl[0] = new RcharLimited(0, '1');
            Array.Copy(Numerics(8, 1), 0, dl, 1, 8);
            var dlf01 = new DriversLicense(dl, this);

            dl = new Rchar[9];
            dl[0] = new RcharLimited(0, '2');
            dl[1] = new RcharLimited(1, '0', '1', '2', '3', '4');
            Array.Copy(Numerics(7, 2), 0, dl, 2, 7);
            var dlf02 = new DriversLicense(dl, this);

            dlFormats = new[] {dlf00, dlf01, dlf02};
        }
    }
}