using System;

namespace Notes.Chemistry.Elements.Bonds
{
    public abstract class BondBase : IBond
    {
        private readonly IElement _atom1;
        private readonly IElement _atom2;
        protected BondBase(IElement atom1, IElement atom2)
        {
            atom1 = atom1 ?? throw new ArgumentNullException(nameof(atom1));
            atom2 = atom2 ?? throw new ArgumentNullException(nameof(atom2));

            if (atom1.Equals(atom2))
                throw new ArgumentException("An atom cannot bind to itself");

            _atom1 = atom1;
            _atom2 = atom2;
        }

        public IElement GetBondedAtom(IElement fromHere)
        {
            if (fromHere == null)
                return _atom1;

            if (fromHere.Equals(_atom1))
                return _atom2;
            if (fromHere.Equals(_atom2))
                return _atom1;

            return null;
        }
        public override int GetHashCode()
        {
            return GetType().Name.GetHashCode() + _atom1.GetHashCode() + _atom2.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var bond = obj as IBond;
            if (bond == null)
                return base.Equals(obj);

            var objAtom1 = bond.GetBondedAtom(null);
            var objAtom2 = bond.GetBondedAtom(objAtom1);

            return obj?.GetType() == GetType() && _atom1.Equals(objAtom1) && _atom2.Equals(objAtom2);
        }
    }
}
