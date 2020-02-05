using System;
using Notes.Chemistry.Elements.ElectronCfg.Orbitals;

namespace Notes.Chemistry.Elements
{
    public interface IOrbitalGroup : IComparable<IOrbitalGroup>
    {
        IShell Shell { get; }
        int? AddElectron();
        int? RemoveElectron();
        int GetCountElectrons();
        Orbital[] AssignedElectrons { get; }
    }
}
