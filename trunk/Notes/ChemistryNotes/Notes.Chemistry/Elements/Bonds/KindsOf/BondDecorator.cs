using System;

namespace Notes.Chemistry.Elements.Bonds.KindsOf
{
    public abstract class BondDecorator : IBond
    {
        private readonly IBond _toDecorate;

        protected BondDecorator(IBond toDecorate)
        {
            _toDecorate = toDecorate ?? throw new ArgumentNullException(nameof(toDecorate));
        }

        public IElement GetBondedAtom(IElement fromHere)
        {
            return _toDecorate.GetBondedAtom(fromHere);
        }

        protected internal bool TestHasType(Type type)
        {
            return GetBond(type) != null;
        }

        protected internal IBond GetBond(Type type)
        {
            if (GetType() == type)
                return this;

            if(_toDecorate.GetType() == type)
                return _toDecorate;

            var decorator = _toDecorate as BondDecorator;
            return decorator?.GetBond(type);
        }

        public override int GetHashCode()
        {
            var atom1 = _toDecorate.GetBondedAtom(null);
            var atom2 = _toDecorate.GetBondedAtom(atom1);
            return GetType().Name.GetHashCode() + atom1.GetHashCode() + atom2.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var bond = obj as IBond;
            if (bond == null)
                return base.Equals(obj);

            var atom1 = _toDecorate.GetBondedAtom(null);
            var atom2 = _toDecorate.GetBondedAtom(atom1);

            var objAtom1 = bond.GetBondedAtom(null);
            var objAtom2 = bond.GetBondedAtom(objAtom1);

            return obj?.GetType() == GetType() && atom1.Equals(objAtom1) && atom2.Equals(objAtom2);
        }
    }
}