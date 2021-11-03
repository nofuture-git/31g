namespace Notes.Chemistry.Elements.ElectronCfg.Orbitals
{
    /// <summary>
    /// The fifth orbital group after <see cref="f_OrbitalGroup"/>.
    /// </summary>
    public class g_OrbitalGroup : OrbitalGroupBase
    {
        public g_OrbitalGroup(IShell myShell) : this(myShell, 9)
        {
        }

        public g_OrbitalGroup(IShell myShell, int count) : base(myShell, count)
        {
        }

        protected internal override string Abbrev => "g";

        public override int CompareTo(IOrbitalGroup other)
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
                case f_OrbitalGroup _:
                    return 1;
                case g_OrbitalGroup _:
                    return 0;
                default:
                    return -1;
            }
        }
    }
}
