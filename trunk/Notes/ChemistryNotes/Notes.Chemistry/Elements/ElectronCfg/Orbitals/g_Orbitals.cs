﻿using Notes.Chemistry.Elements.ElectronCfg.Shells;

namespace Notes.Chemistry.Elements.ElectronCfg.Orbitals
{
    public class g_Orbitals : OrbitalsBase
    {
        public g_Orbitals(IShell myShell) : this(myShell, 9)
        {
        }

        public g_Orbitals(IShell myShell, int count) : base(myShell, count)
        {
        }

        protected internal override string Abbrev => "g";

        public override int CompareTo(IOrbitals other)
        {
            var bc = base.CompareShells(other);
            if (bc != null)
                return bc.Value;

            switch (other)
            {
                case s_Orbitals _:
                case sp_hybridizedOrbitals _:
                case p_Orbitals _:
                case d_Orbitals _:
                case f_Orbitals _:
                    return 1;
                case g_Orbitals _:
                    return 0;
                default:
                    return -1;
            }
        }
    }
}
