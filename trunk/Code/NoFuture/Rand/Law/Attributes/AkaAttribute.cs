using System;

namespace NoFuture.Rand.Law.Attributes
{
    public class AkaAttribute : Attribute
    {
        public string OtherName { get; set; }

        public AkaAttribute(string othername)
        {
            OtherName = othername;
        }
    }
}