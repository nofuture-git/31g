namespace Notes.Chemistry.Elements.Bonds
{
    /// <summary>
    /// The bond where electronegativity difference is
    /// large (greater than 2) and the bond is between a cation and an anion.
    /// </summary>
    public class Ionic : BondBase
    {
        public Ionic(IElement atom1, IElement atom2) : base(atom1, atom2)
        {
            
        }
    }
}
