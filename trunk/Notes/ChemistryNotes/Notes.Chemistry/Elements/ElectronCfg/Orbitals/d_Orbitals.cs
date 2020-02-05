using Notes.Chemistry.Elements.ElectronCfg.Shells;

namespace Notes.Chemistry.Elements.ElectronCfg.Orbitals
{
    public class d_Orbitals : OrbitalsBase
    {
        public d_Orbitals(IShell myShell) : this(myShell, 5)
        {
        }

        public d_Orbitals(IShell myShell, int count) : base(myShell, count)
        {
        }

        protected internal override string Abbrev => "d";

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
                    return 1;
                case d_Orbitals _:
                    return 0;
                default:
                    return -1;
            }
        }
    }
}
