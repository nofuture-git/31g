using Notes.Chemistry.Elements.ElectronCfg.Orbitals;

namespace Notes.Chemistry.Elements.ElectronCfg.Shells
{
    public class OShell : ShellBase
    {
        public OShell(IElement element) : base(element)
        {
            Orbitals.Add(new s_Orbitals(this));
            Orbitals.Add(new p_Orbitals(this));
            Orbitals.Add(new d_Orbitals(this));
            Orbitals.Add(new f_Orbitals(this));
            Orbitals.Add(new g_Orbitals(this));
        }

        public override int CompareTo(IShell other)
        {
            switch (other)
            {
                case KShell _:
                case LShell _:
                case MShell _:
                case NShell _:
                    return 1;
                case OShell _:
                    return 0;
                default:
                    return -1;
            }
        }

        protected internal override string Abbrev => "5";
    }
}
