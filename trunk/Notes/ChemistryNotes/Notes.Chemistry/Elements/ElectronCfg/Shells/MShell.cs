using Notes.Chemistry.Elements.ElectronCfg.Orbitals;

namespace Notes.Chemistry.Elements.ElectronCfg.Shells
{
    public class MShell : ShellBase
    {
        public MShell(IElement element) : base(element)
        {
            Orbits.Add(new s_Orbitals(this));
            Orbits.Add(new p_Orbitals(this));
            Orbits.Add(new d_Orbitals(this));

        }
        public override int CompareTo(ShellBase other)
        {
            switch (other)
            {
                case KShell _:
                case LShell _:
                    return 1;
                case MShell _:
                    return 0;
                default:
                    return -1;
            }
        }

        protected internal override string Abbrev => "3";

    }
}
