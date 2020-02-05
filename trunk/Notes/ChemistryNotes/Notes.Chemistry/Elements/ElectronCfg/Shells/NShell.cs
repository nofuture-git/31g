using Notes.Chemistry.Elements.ElectronCfg.Orbitals;

namespace Notes.Chemistry.Elements.ElectronCfg.Shells
{
    public class NShell : ShellBase
    {
        public NShell(IElement element) : base(element)
        {
            Orbits.Add(new s_Orbitals(this));
            Orbits.Add(new p_Orbitals(this));
            Orbits.Add(new d_Orbitals(this));
            Orbits.Add(new f_Orbitals(this));
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
