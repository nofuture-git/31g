﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
