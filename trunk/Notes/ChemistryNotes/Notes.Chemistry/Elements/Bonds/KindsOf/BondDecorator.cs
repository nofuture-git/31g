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

        public IElement Atom1 => _toDecorate.Atom1;

        public IElement Atom2 => _toDecorate.Atom2;

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
    }
}