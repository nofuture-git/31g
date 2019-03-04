using System;

namespace NoFuture.Rand.Law.Attributes
{
    public class NoteAttribute : Attribute
    {
        public string[] Notes { get; }

        public NoteAttribute(params string[] notes)
        {
            Notes = notes;
        }
    }
}