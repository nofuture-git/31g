using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.Census
{
    /// <summary>
    /// Metropolitan Statistical Area - a geographical region with 
    /// a relatively high population density at its core and 
    /// close economic ties throughout the area.
    /// [https://en.wikipedia.org/wiki/Metropolitan_statistical_area]
    /// </summary>
    [Serializable]
    public class MStatArea : Identifier
    {
        public override string Abbrev => "MSA";
        public UrbanCentric MsaType { get; set; }
    }
}