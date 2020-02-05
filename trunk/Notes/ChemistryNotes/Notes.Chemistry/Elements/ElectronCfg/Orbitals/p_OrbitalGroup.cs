namespace Notes.Chemistry.Elements.ElectronCfg.Orbitals
{
    public class p_OrbitalGroup : OrbitalGroupBase
    {
        public p_OrbitalGroup(IShell myShell) : this(myShell, 3)
        {
        }

        public p_OrbitalGroup(IShell myShell, int count) : base(myShell, count)
        {
        }

        protected internal override string Abbrev => "p";

        public override int CompareTo(IOrbitalGroup other)
        {
            var bc = base.CompareShells(other);
            if (bc != null)
                return bc.Value;

            switch (other)
            {
                case s_OrbitalGroup _:
                case sp_hybridizedOrbitalGroup _:
                    return 1;
                case p_OrbitalGroup _:
                    return 0;
                default:
                    return -1;
            }
        }
    }
}
