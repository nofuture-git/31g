using System;
using System.Collections.Generic;

namespace Notes.Chemistry.Elements
{
    public interface IShell : IComparable<IShell>
    {
        IElement Element { get; }
        int ShellMaxElectrons { get; }
        SortedSet<IOrbitals> Orbits { get; }
        int? AddElectron();
        int? RemoveElectron();
        int GetCountElectrons();
    }
}
