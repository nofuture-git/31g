using System;
using Notes.Chemistry.Elements.ElectronCfg.Orbitals;

namespace Notes.Chemistry.Elements.ElectronCfg.Shells
{
    public class LShell : ShellBase
    {
        public LShell(IElement element) : base(element)
        {
            Orbitals.Add(new s_OrbitalGroup(this));
            Orbitals.Add(new p_OrbitalGroup(this));
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
            var hybridOrbit = new sp_hybridizedOrbitalGroup(this, hybridizedCount);

            var electronCount = GetCountElectrons();
            for (var i = 0; i < electronCount; i++)
            {
                hybridOrbit.AddElectron();
                RemoveElectron();
            }

            Orbitals.Clear();
            Orbitals.Add(hybridOrbit);
        }
    }
}
