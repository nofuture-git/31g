using System;

namespace NoFuture.Rand.Law.Attributes
{
    [EtymologyNote("Latin", "conferatur", "compare")]
    public class CfAttribute : Attribute
    {
        public string[] ContrastWith { get; set; }

        public CfAttribute(params string[] contrastWith)
        {
            contrastWith = contrastWith ?? new string[] { };
            ContrastWith = contrastWith;
        }
    }
}
