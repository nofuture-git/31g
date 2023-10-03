using System;
using NoFuture.Law;

namespace NoFuture.Law.Criminal.US.Elements.Homicide
{
    /// <summary>
    /// A in-time limiting factor in which the homicide occurs within
    /// a specified time span
    /// </summary>
    public interface IHomicideConcurrance : ITempore
    {
        DateTime? TimeOfTheDeath { get; set; }
    }
}
