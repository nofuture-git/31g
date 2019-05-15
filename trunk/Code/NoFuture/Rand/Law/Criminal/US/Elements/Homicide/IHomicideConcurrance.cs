using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Law.Criminal.Homicide.US.Elements
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
