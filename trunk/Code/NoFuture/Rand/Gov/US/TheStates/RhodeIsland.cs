using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class RhodeIsland : UsState
    {
        public RhodeIsland() : base("RI")
        {
            var dl = new Rchar[7];
            dl[0] = new RcharLimited(0, 'V');
            Array.Copy(Numerics(6,1),0,dl,1,6);
            dlFormats = new[] {new DriversLicense(dl, this), new DriversLicense(Numerics(7), this) };
        }
    }
}