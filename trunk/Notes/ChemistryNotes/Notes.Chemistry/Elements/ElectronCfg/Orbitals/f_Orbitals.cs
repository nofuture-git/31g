using Notes.Chemistry.Elements.ElectronCfg.Shells;

namespace Notes.Chemistry.Elements.ElectronCfg.Orbitals
{
    public class f_Orbitals : OrbitalsBase
    {
        public f_Orbitals(ShellBase myShell) : this(myShell, 7)
        {

        }
        public f_Orbitals(ShellBase myShell, int count) : base(myShell, count)
        {

        }

        protected internal override string Abbrev => "f";

        public override int CompareTo(OrbitalsBase other)
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
                    return 1;
                case f_Orbitals _:
                    return 0;
                default:
                    return -1;
            }
        }
    }
}
