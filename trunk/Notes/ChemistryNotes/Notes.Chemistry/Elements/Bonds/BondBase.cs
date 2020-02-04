using System;

namespace Notes.Chemistry.Elements.Bonds
{
    public abstract class BondBase : IBond
    {
        protected BondBase(IElement atom1, IElement atom2)
        {
            atom1 = atom1 ?? throw new ArgumentNullException(nameof(atom1));
            atom2 = atom2 ?? throw new ArgumentNullException(nameof(atom2));

            if (atom1.Equals(atom2))
                throw new ArgumentException("An atom cannot bind to itself");

            Atom1 = atom1;
            Atom2 = atom2;
        }

        public IElement Atom1 { get; }
        public IElement Atom2 { get; }
    }
}
