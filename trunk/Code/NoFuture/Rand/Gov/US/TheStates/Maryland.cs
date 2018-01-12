﻿using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Maryland : UsState
    {
        public Maryland() : base("MD")
        {
            var dl = new Rchar[13];
            dl[0] = new RcharUAlpha(0);
            Array.Copy(Numerics(12, 1), 0, dl, 1, 12);

            dlFormats = new[] { new DriversLicense(dl, this) };
        }
    }
}