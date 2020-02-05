using Notes.Chemistry.Elements.ElectronCfg.Orbitals;

namespace Notes.Chemistry.Elements.ElectronCfg.Shells
{
    public class KShell : ShellBase
    {
        public KShell(IElement element) : base(element)
        {
            Orbits.Add(new s_Orbitals(this));
        }

        public override int CompareTo(IShell other)
        {
            return other is KShell ? 0 : -1;
        }

        protected internal override string Abbrev => "1";
    }
}
