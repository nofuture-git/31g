﻿using Notes.Chemistry.Elements.ElectronCfg.Orbitals;

namespace Notes.Chemistry.Elements.ElectronCfg.Shells
{
    /// <summary>
    /// The third shell which has one s-orbital, three p-orbitals and five d-orbitals
    /// </summary>
    public class MShell : ShellBase
    {
        public MShell(IElement element) : base(element)
        {
            Orbitals.Add(new s_OrbitalGroup(this));
            Orbitals.Add(new p_OrbitalGroup(this));
            Orbitals.Add(new d_OrbitalGroup(this));

        }
        public override int CompareTo(IShell other)
        {
            switch (other)
            {
                case KShell _:
                case LShell _:
                    return 1;
                case MShell _:
                    return 0;
                default:
                    return -1;
            }
        }

        protected internal override string Abbrev => "3";

    }
}
