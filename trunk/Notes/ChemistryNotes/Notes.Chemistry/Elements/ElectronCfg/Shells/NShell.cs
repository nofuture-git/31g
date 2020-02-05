using Notes.Chemistry.Elements.ElectronCfg.Orbitals;

namespace Notes.Chemistry.Elements.ElectronCfg.Shells
{
    public class NShell : ShellBase
    {
        public NShell(IElement element) : base(element)
        {
            Orbitals.Add(new s_OrbitalGroup(this));
            Orbitals.Add(new p_OrbitalGroup(this));
            Orbitals.Add(new d_OrbitalGroup(this));
            Orbitals.Add(new f_OrbitalGroup(this));
        }

        public override int CompareTo(IShell other)
        {
            switch (other)
            {
                case KShell _:
                case LShell _:
                case MShell _:
                    return 1;
                case NShell _:
                    return 0;
                default:
                    return -1;
            }
        }

        protected internal override string Abbrev => "4";
    }
}
