using System;
using NoFuture.Rand.Law;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Homicide
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
