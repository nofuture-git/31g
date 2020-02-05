using Notes.Chemistry.Elements.ElectronCfg.Shells;

namespace Notes.Chemistry.Elements.ElectronCfg.Orbitals
{
    public class p_Orbitals : OrbitalsBase
    {
        public p_Orbitals(IShell myShell) : this(myShell, 3)
        {
        }

        public p_Orbitals(IShell myShell, int count) : base(myShell, count)
        {
        }

        protected internal override string Abbrev => "p";

        public override int CompareTo(IOrbitals other)
        {
            var bc = base.CompareShells(other);
            if (bc != null)
                return bc.Value;

            switch (other)
            {
                case s_Orbitals _:
                case sp_hybridizedOrbitals _:
                    return 1;
                case p_Orbitals _:
                    return 0;
                default:
                    return -1;
            }
        }
    }
}
