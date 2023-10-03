using System;

namespace NoFuture.Law.Attributes
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