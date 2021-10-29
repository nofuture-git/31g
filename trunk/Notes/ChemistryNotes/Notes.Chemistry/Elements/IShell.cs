using System;
using System.Collections.Generic;

namespace Notes.Chemistry.Elements
{
    /// <summary>
    /// Indicates the energy level of a particular electron.
    /// </summary>
    /// <remarks>
    /// A shell can be thought of as the floor of an apartment
    /// high-rise while the orbital is the particular apartments of a floor.
    /// </remarks>
    /// <remarks>
    /// The outermost shell of an atom is called &quot;valence shell&quot;
    /// </remarks>
    public interface IShell : IComparable<IShell>
    {
        IElement Element { get; }
        int ShellMaxElectrons { get; }
        SortedSet<IOrbitalGroup> Orbitals { get; }
        int? AddElectron();
        int? RemoveElectron();
        int GetCountElectrons();
    }
}
