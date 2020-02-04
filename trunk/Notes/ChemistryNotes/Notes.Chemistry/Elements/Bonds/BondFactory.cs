using System;

namespace Notes.Chemistry.Elements.Bonds
{
    public static class BondFactory
    {
        public static IBond CreateBond(IElement atom1, IElement atom2)
        {
            if(atom1 == null)
                throw new ArgumentNullException(nameof(atom1));
            if(atom2 == null)
                throw new ArgumentNullException(nameof(atom2));

            if(atom1.IsSameElement(atom2))
                return new PurelyCovalent(atom1, atom2);

            var electronegativityDiff = Math.Abs(atom1.Electronegativity - atom2.Electronegativity);

            if (electronegativityDiff <= 2.0D)
                return new PolarCovalent(atom1, atom2);

            return new Ionic(atom1, atom2);
        }
    }
}
