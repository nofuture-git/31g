using System;
using System.Collections.Generic;
using Notes.Chemistry.Elements.ElectronCfg.Shells;

namespace Notes.Chemistry.Elements
{
    public interface IElement
    {
        Guid AtomId { get; }
        byte AtomicNumber { get; }
        string Symbol { get; }
        string Name { get; }
        double AtomicMass { get; }
        double Electronegativity { get; }
        Phase RoomTempPhase { get; }
        bool IsArtificial { get; }
        bool IsRadioactive { get; }
        SortedSet<IShell> Shells { get; }
        double GetGrams(double countOfAtoms);
        double GetAtoms(double grams);
        bool IsCation { get; }
        bool IsAnion { get; }
        bool IsIon { get; }
        IShell ValenceShell { get; }

        int MaxElectrons { get; }
        int? AddElectron();
        int? RemoveElectron();
        int CountElectrons { get; }
        string PrintElectronShellCfg(bool shortVersion = true);
    }
}
