using System;
using Notes.Chemistry.Elements.ElectronCfg.Orbitals;

namespace Notes.Chemistry.Elements
{
    /// <summary>
    /// A grouping of <see cref="Orbital"/>&apos;s. 
    /// </summary>
    /// <remarks>
    /// While an <see cref="s_OrbitalGroup"/> has only one <see cref="Orbital"/>,
    /// a <see cref="p_OrbitalGroup"/> will have 3 orbitals (one along x-axis,
    /// one on y-axis, and one on z-axis). Likewise, a <see cref="d_OrbitalGroup"/>
    /// will have 5 orbitals, four being clover-shaped and one being like a p-orbitals
    /// double tear-drop passing straight through a doughnut.
    /// </remarks>
    public interface IOrbitalGroup : IComparable<IOrbitalGroup>
    {
        IShell Shell { get; }
        int? AddElectron();
        int? RemoveElectron();
        int GetCountElectrons();
        Orbital[] AssignedElectrons { get; }
    }
}
