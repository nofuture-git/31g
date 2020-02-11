namespace Notes.Chemistry.Elements.Bonds.KindsOf
{
    public class TripleCovalent : BondDecorator
    {
        protected internal TripleCovalent(IBond toDecorate) : base(toDecorate)
        {
            var atom1 = toDecorate.GetBondedAtom(null);
            var atom2 = toDecorate.GetBondedAtom(atom1);

            atom1.AddElectron();
            atom2.AddElectron();

            atom1.AddElectron();
            atom2.AddElectron();
        }
    }
}
