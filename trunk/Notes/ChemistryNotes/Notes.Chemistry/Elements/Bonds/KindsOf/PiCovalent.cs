namespace Notes.Chemistry.Elements.Bonds.KindsOf
{
    /// <summary>
    /// The covalent bond formed by, typically, p-orbitals overlapping laterally.
    /// </summary>
    /// <remarks>
    /// <see cref="DoubleCovalent"/> and <see cref="TripleCovalent"/>
    /// are typically formed of this kind of covalent bond.
    /// </remarks>
    public class PiCovalent : BondDecorator
    {
        protected internal PiCovalent(IBond bond) : base(bond)
        {
        }
    }
}