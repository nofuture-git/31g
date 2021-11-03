namespace Notes.Chemistry.Elements.ElectronCfg.Orbitals
{
    /// <summary>
    /// The third orbital group after <see cref="p_OrbitalGroup"/> and is shaped
    /// like a clover.
    /// </summary>
    public class d_OrbitalGroup : OrbitalGroupBase
    {
        public d_OrbitalGroup(IShell myShell) : this(myShell, 5)
        {
        }

        public d_OrbitalGroup(IShell myShell, int count) : base(myShell, count)
        {
        }

        protected internal override string Abbrev => "d";

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
                    return 1;
                case d_OrbitalGroup _:
                    return 0;
                default:
                    return -1;
            }
        }
    }
}
