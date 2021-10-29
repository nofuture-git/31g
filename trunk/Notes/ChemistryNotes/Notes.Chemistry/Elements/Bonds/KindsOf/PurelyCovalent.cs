namespace Notes.Chemistry.Elements.Bonds.KindsOf
{
    /// <summary>
    /// The bond formed when the electronegativity difference is zero
    /// </summary>
    public class PurelyCovalent : BondDecorator
    {
        protected internal PurelyCovalent(IBond bond) : base(bond)
        {
        }
    }
}
