using System;
using System.Linq;

namespace NoFuture.Rand.Law.Attributes
{
    public class AkaAttribute : Attribute
    {
        public string OtherName { get; set; }

        public string[] OtherNames { get; set; }

        public AkaAttribute(string othername)
        {
            OtherNames = new[] {othername};
            OtherName = othername;
        }

        public AkaAttribute(params string[] othernames)
        {
            othernames = othernames ?? new string[] { };
            OtherName = othernames.FirstOrDefault();
            OtherNames = othernames;
        }
    }
}