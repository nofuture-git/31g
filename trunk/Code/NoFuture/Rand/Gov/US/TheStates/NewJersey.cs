using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    public class NewJersey : UsState
    {
        public NewJersey() : base("NJ")
        {
            var dl = new Rchar[15];
            dl[0] = new RcharUAlpha(0);
            Array.Copy(Numerics(14,1),0,dl,1,14);
            dlFormats = new[] {new DriversLicense(dl, this) };
        }
    }
}