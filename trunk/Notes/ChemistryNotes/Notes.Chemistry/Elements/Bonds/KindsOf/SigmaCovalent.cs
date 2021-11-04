namespace Notes.Chemistry.Elements.Bonds.KindsOf
{
    /// <summary>
    /// The strongest covalent bond where, when viewed dow the bond axis has a circular symmetry.
    /// </summary>
    public class SigmaCovalent : BondDecorator
    {
        protected internal SigmaCovalent(IBond bond) : base(bond)
        {
        }
    }
}
