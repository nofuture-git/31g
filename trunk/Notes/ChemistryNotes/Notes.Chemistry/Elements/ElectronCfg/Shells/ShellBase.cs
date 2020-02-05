using System;
using System.Collections.Generic;
using Notes.Chemistry.Elements.ElectronCfg.Orbitals;

namespace Notes.Chemistry.Elements.ElectronCfg.Shells
{
    public abstract class ShellBase : IShell
    {
        public IElement Element { get; }

        protected ShellBase(IElement element)
        {
            Element = element ?? throw new NotImplementedException();
        }

        public int ShellMaxElectrons
        {
            get
            {
                var max = 0;
                foreach (var orbit in Orbitals)
                {
                    max += orbit.AssignedElectrons.Length * 2;
                }

                return max;
            }
        }

        public SortedSet<IOrbitals> Orbitals { get; } = new SortedSet<IOrbitals>();

        public abstract int CompareTo(IShell other);

        protected internal abstract string Abbrev { get; }

        public virtual int? AddElectron()
        {
            foreach (var orbit in Orbitals)
            {
                var add = orbit.AddElectron();
                if (add != null)
                    return add;
            }

            return null;
        }

        public virtual int? RemoveElectron()
        {
            foreach (var orbit in Orbitals.Reverse())
            {
                var remove = orbit.RemoveElectron();
                if (remove != null)
                    return remove;
            }

            return null;
        }

        public virtual int GetCountElectrons()
        {
            var count = 0;
            foreach (var orbit in Orbitals)
                count += orbit.GetCountElectrons();
            return count;
        }

        public override bool Equals(object obj)
        {
            var shell = obj as IShell;
            if (shell == null)
                return base.Equals(obj);

            return obj?.GetType() == GetType() && Element.Equals(shell.Element);
        }
        public override int GetHashCode()
        {
            return GetType().Name.GetHashCode() + Element.GetHashCode();
        }

        protected internal virtual string[] GetElectronCfgShort()
        {
            return GetElectronCfg();
        }

        protected internal virtual string[] GetElectronCfgLong()
        {
            return GetElectronCfg(false);
        }

        protected internal virtual string[] GetElectronCfg(bool shortVersion = true)
        {
            var strs = new List<string>();
            foreach (var iOrbit in Orbitals)
            {
                var orbit = iOrbit as OrbitalGroupBase;
                if(orbit == null)
                    continue;
                var oos = shortVersion ? orbit.GetElectronCfgShort() : orbit.GetElectronCfgLong();
                foreach (var oo in oos)
                    strs.Add($"{Abbrev}{oo}");
            }

            return strs.ToArray();
        }
    }
}
