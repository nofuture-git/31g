namespace Notes.Chemistry.Elements.Bonds
{
    /// <summary>
    /// A kind of bond in which electrons are shared.
    /// </summary>
    public class Covalent : BondBase
    {
        public Covalent(IElement atom1, IElement atom2) : base(atom1, atom2)
        {
            atom1.AddElectron();
            atom2.AddElectron();
        }
    }
}
