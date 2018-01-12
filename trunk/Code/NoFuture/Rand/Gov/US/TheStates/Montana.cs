using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Montana : UsState
    {
        public Montana() : base("MT")
        {
            var dl = new Rchar[9];
            dl[0] = new RcharUAlpha(0);
            dl[1] = new RcharNumeric(1);
            dl[2] = new RcharAlphaNumeric(2);
            Array.Copy(Numerics(6, 3), 0, dl, 3, 6);
            dlFormats = new[] { new DriversLicense(dl, this) };
        }
    }
}