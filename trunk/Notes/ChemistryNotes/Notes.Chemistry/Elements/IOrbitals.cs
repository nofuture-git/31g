using System;
using Notes.Chemistry.Elements.ElectronCfg.Orbitals;

namespace Notes.Chemistry.Elements
{
    public interface IOrbitals : IComparable<IOrbitals>
    {
        IShell Shell { get; }
        int? AddElectron();
        int? RemoveElectron();
        int GetCountElectrons();
        Orbital[] AssignedElectrons { get; }
    }
}
