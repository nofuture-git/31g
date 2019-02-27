using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Intent
{
    /// <summary>
    /// the willful intent to do harm
    /// </summary>
    [Aka("criminal intent", "guilty mind", "vicious will")]
    public interface IMensRea : IProsecution, IElement, IComparable
    {
        /// <summary>
        /// Determines if this criminal intent is valid 
        /// when combined with the particular <see cref="criminalAct"/>
        /// </summary>
        bool CompareTo(IActusReus criminalAct, params ILegalPerson[] persons);
    }
}
