using Notes.Chemistry.Elements.ElectronCfg.Orbitals;

namespace Notes.Chemistry.Elements.ElectronCfg.Shells
{
    /// <summary>
    /// The first shell which has only one s-orbital
    /// </summary>
    public class KShell : ShellBase
    {
        public KShell(IElement element) : base(element)
        {
            Orbitals.Add(new s_OrbitalGroup(this));
        }

        public override int CompareTo(IShell other)
        {
            return other is KShell ? 0 : -1;
        }

        protected internal override string Abbrev => "1";
    }
}
