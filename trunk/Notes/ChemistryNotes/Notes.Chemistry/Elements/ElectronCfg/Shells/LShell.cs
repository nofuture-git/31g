using System;
using Notes.Chemistry.Elements.ElectronCfg.Orbitals;

namespace Notes.Chemistry.Elements.ElectronCfg.Shells
{
    public class LShell : ShellBase
    {
        public LShell(IElement element) : base(element)
        {
            Orbits.Add(new s_Orbitals(this));
            Orbits.Add(new p_Orbitals(this));
        }

        public override int CompareTo(IShell other)
        {
            switch (other)
            {
                case KShell _:
                    return 1;
                case LShell _:
                    return 0;
                default:
                    return -1;
            }
        }

        protected internal override string Abbrev => "2";

        public virtual void HybridizeOrbits(int hybridizedCount)
        {
            var hybridOrbit = new sp_hybridizedOrbitals(this, hybridizedCount);

            var electronCount = GetCountElectrons();
            for (var i = 0; i < electronCount; i++)
            {
                hybridOrbit.AddElectron();
                RemoveElectron();
            }

            Orbits.Clear();
            Orbits.Add(hybridOrbit);
        }
    }
}
