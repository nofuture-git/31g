﻿namespace NoFuture.Rand.Gov.TheStates
{
    public class Tennessee : UsState
    {
        public Tennessee() : base("TN")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9), this), new DriversLicense(Numerics(8), this) };
        }
    }
}