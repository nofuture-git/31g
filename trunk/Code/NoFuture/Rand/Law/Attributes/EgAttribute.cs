using System;

namespace NoFuture.Law.Attributes
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
