using System;

namespace NoFuture.Rand.Gov.US.Census
{
    /// <summary>
    /// A Combined Statistical Area, also known as a Core Based Statistical Area
    /// </summary>
    [Serializable]
    public class ComboMStatArea : MStatArea
    {
        public override string Abbrev => "CBSA";
    }
}