using System;

namespace NoFuture.Rand.Law.Attributes
{
    public class EgAttribute : Attribute
    {
        public string[] Examples { get; }

        public EgAttribute(params string[] notes)
        {
            Examples = notes;
        }
    }
}
