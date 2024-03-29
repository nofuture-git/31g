﻿using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Hawaii : UsState
    {
        public Hawaii() : base("HI")
        {
            var dl = new Rchar[9];
            dl[0] = new RcharLimited(0,'H');
            Array.Copy(Numerics(8, 1), 0, dl, 1, 8);

            dlFormats = new[] { new DriversLicense(dl, this), new DriversLicense(Numerics(9), this) };
        }
    }
}