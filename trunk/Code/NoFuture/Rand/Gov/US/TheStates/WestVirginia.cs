using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class WestVirginia : UsState
    {
        public WestVirginia() : base("WV")
        {
            var dl = new Rchar[7];
            dl[0] = new RcharLimited(0,'A','B','C','D','E','F','I','S','0','1','X');
            Array.Copy(Numerics(6,1),0,dl,1,6);

            dlFormats = new[] {new DriversLicense(dl, this) };
        }
    }
}