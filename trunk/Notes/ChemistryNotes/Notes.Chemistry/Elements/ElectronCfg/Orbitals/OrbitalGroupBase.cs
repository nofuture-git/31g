﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Notes.Chemistry.Elements.ElectronCfg.Orbitals
{
    public abstract class OrbitalGroupBase : IOrbitalGroup
    {
        public IShell Shell { get; }

        protected internal abstract string Abbrev { get; }

        public Orbital[] AssignedElectrons { get; }

        protected OrbitalGroupBase(IShell myShell)
        {
            Shell = myShell ?? throw new NotImplementedException();
        }

        protected OrbitalGroupBase(IShell myShell, int count) : this(myShell)
        {
            if(count <= 0)
                throw new ArgumentException($"{nameof(count)} must be greater than zero");

            var refactorAssignedElectrons = new Orbital[count];
            for (var i = 0; i < count; i++)
            {
                refactorAssignedElectrons[i] = new Orbital(this);
            }

            AssignedElectrons = refactorAssignedElectrons;
        }

        protected int? CompareShells(IOrbitalGroup other)
        {
            if (other == null)
                return 0;
            var shellCompare = Shell.CompareTo(other.Shell);
            if (shellCompare != 0)
                return shellCompare;
            return null;

        }

        public virtual int? AddElectron()
        {
            return IterateAssignedElectrons(true, electron => electron.IsPresent = true);
        }

        protected internal virtual int? IterateAssignedElectrons(bool isPresent, Action<Electron> op = null)
        {
            var len = AssignedElectrons.Length;

            for (var i = 0; i < len * 2; i++)
            {
                var idx = i % len;
                var orbital = AssignedElectrons[idx];
                var electron = i >= len ? orbital.SpinDown : orbital.SpinUp;
                if (electron.IsPresent == isPresent)
                    continue;
                op?.Invoke(electron);
                
                return i;
            }

            return null;
        }

        public virtual int? RemoveElectron()
        {
            return IterateAssignedElectrons(false, electron => electron.IsPresent = false);
        }

        public virtual int GetCountElectrons()
        {
            var count = 0;
            var len = AssignedElectrons.Length;

            for (var i = 0; i < len * 2; i++)
            {
                var idx = i % len;
                var orbital = AssignedElectrons[idx];
                var electron = i >= len ? orbital.SpinDown : orbital.SpinUp;
                if (electron.IsPresent)
                {
                    count += 1;
                }
            }

            return count;
        }

        public abstract int CompareTo(IOrbitalGroup other);

        public override bool Equals(object obj)
        {
            var orbit = obj as IOrbitalGroup;
            if (orbit == null)
                return base.Equals(obj);

            return obj?.GetType() == GetType() && Shell.Equals(orbit.Shell);
        }
        public override int GetHashCode()
        {
            return GetType().Name.GetHashCode() + Shell.GetHashCode();
        }

        protected internal virtual string[] GetElectronCfgLong()
        {
            var str = new List<string>();

            var abbrev = new[] { "x", "y", "z", "i", "j", "k", "m", "n", "r" };

            for (var i = 0; i < AssignedElectrons.Length; i++)
            {
                var orbital = AssignedElectrons[i];
                var count = new[] { orbital.SpinUp.IsPresent, orbital.SpinDown.IsPresent }.Count(e => e);
                var axis = abbrev[i];
                var obr = orbital.Abbrev ?? Abbrev;
                str.Add($"{obr}_{axis}^{count}");
            }

            return str.ToArray();
        }

        protected internal virtual string[] GetElectronCfgShort()
        {
            return new[] {$"{Abbrev}{GetCountElectrons()}"};
        }
    }
}
