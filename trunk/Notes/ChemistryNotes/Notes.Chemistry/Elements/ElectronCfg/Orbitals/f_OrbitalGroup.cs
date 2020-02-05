namespace Notes.Chemistry.Elements.ElectronCfg.Orbitals
{
    public class f_OrbitalGroup : OrbitalGroupBase
    {
        public f_OrbitalGroup(IShell myShell) : this(myShell, 7)
        {

        }
        public f_OrbitalGroup(IShell myShell, int count) : base(myShell, count)
        {

        }

        protected internal override string Abbrev => "f";

        public override int CompareTo(IOrbitals other)
        {
            var bc = base.CompareShells(other);
            if (bc != null)
                return bc.Value;

            switch (other)
            {
                case s_OrbitalGroup _:
                case sp_hybridizedOrbitalGroup _:
                case p_OrbitalGroup _:
                case d_OrbitalGroup _:
                    return 1;
                case f_OrbitalGroup _:
                    return 0;
                default:
                    return -1;
            }
        }
    }
}
