namespace Notes.Chemistry.Elements.ElectronCfg.Orbitals
{
    public class s_OrbitalGroup : OrbitalGroupBase
    {
        public s_OrbitalGroup(IShell myShell) : this(myShell, 1)
        {
        }

        public s_OrbitalGroup(IShell myShell, int count) : base(myShell, count)
        {
        }

        protected internal override string Abbrev => "s";

        public override int? AddElectron()
        {
            if (AssignedElectrons[0].SpinUp.IsPresent == false)
            {
                AssignedElectrons[0].SpinUp.IsPresent = true;
                return 1;
            }

            if (AssignedElectrons[0].SpinDown.IsPresent == false)
            {
                AssignedElectrons[0].SpinDown.IsPresent = true;
                return 2;
            }

            return null;
        }

        public override int? RemoveElectron()
        {
            if (AssignedElectrons[0].SpinDown.IsPresent)
            {
                AssignedElectrons[0].SpinDown.IsPresent = false;
                return 1;
            }

            if (AssignedElectrons[0].SpinUp.IsPresent)
            {
                AssignedElectrons[0].SpinUp.IsPresent = false;
                return 0;
            }

            return null;
        }

        public override int CompareTo(IOrbitals other)
        {
            var bc = base.CompareShells(other);
            if (bc != null)
                return bc.Value;

            switch (other)
            {
                case s_OrbitalGroup _:
                    return 0;
                default:
                    return -1;
            }
        }

        protected internal override string[] GetElectronCfgLong()
        {
            return GetElectronCfgShort();
        }
    }
}
