using System;

namespace NoFuture.Rand.Law.Attributes
{
    public class AkaAttribute : Attribute
    {
        public string[] OtherNames { get; set; }

        public AkaAttribute(params string[] othernames)
        {
            othernames = othernames ?? new string[] { };
            OtherNames = othernames;
        }
    }
}