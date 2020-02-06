using System;
using Notes.Chemistry.Elements.Bonds.KindsOf;

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

            var electronegativityDiff = Math.Abs(atom1.Electronegativity - atom2.Electronegativity);

            if(electronegativityDiff > 2)
                return new Ionic(atom1, atom2);

            if (atom1.IsSameElement(atom2))
            {
                return new Covalent(atom1, atom2)
                           .AsPurelyCovalent()
                           .AsSigmaBond();

            }

            return new Covalent(atom1, atom2)
                       .AsPolarCovalent();
        }

        public static IBond AsSigmaBond(this IBond bond)
        {
            return new SigmaCovalent(bond);
        }

        public static IBond AsPiBond(this IBond bond)
        {
            return new PiCovalent(bond);
        }

        public static IBond AsPolarCovalent(this IBond bond)
        {
            return new PolarCovalent(bond);
        }

        public static IBond AsPurelyCovalent(this IBond bond)
        {
            return new PurelyCovalent(bond);
        }

        public static IBond AsSingleBond(this IBond bond)
        {
            bond = bond ?? throw new ArgumentNullException(nameof(bond));

            if(bond.Is(typeof(DoubleCovalent)))
                throw new ArgumentException($"{bond.GetBondElementNames()} " +
                                            $"is already declared as a {nameof(DoubleCovalent)}");
            if (bond.Is(typeof(TripleCovalent)))
                throw new ArgumentException($"{bond.GetBondElementNames()} " +
                                            $"is already declared as a {nameof(TripleCovalent)}");

            return new SingleCovalent(bond);
        }

        public static IBond AsDoubleBond(this IBond bond)
        {
            if (bond.Is(typeof(SingleCovalent)))
                throw new ArgumentException($"{bond.GetBondElementNames()} " +
                                            $"is already declared as a {nameof(SingleCovalent)}");
            if (bond.Is(typeof(TripleCovalent)))
                throw new ArgumentException($"{bond.GetBondElementNames()} " +
                                            $"is already declared as a {nameof(TripleCovalent)}");
            return new DoubleCovalent(bond);
        }

        public static IBond AsTripleBond(this IBond bond)
        {
            if (bond.Is(typeof(DoubleCovalent)))
                throw new ArgumentException($"{bond.GetBondElementNames()} " +
                                            $"is already declared as a {nameof(DoubleCovalent)}");
            if (bond.Is(typeof(SingleCovalent)))
                throw new ArgumentException($"{bond.GetBondElementNames()} " +
                                            $"is already declared as a {nameof(SingleCovalent)}");
            return new TripleCovalent(bond);
        }

        public static T GetBond<T>(this IBond bond) where T : IBond
        {
            if (bond == null)
                return default(T);
            if (bond is T bondAsT)
                return bondAsT;

            var decorator = bond as BondDecorator;
            var searchBond = decorator?.GetBond(typeof(T));
            if (searchBond == null)
                return default(T);

            return (T) searchBond;
        }

        public static bool Is(this IBond bond, Type type)
        {
            if (bond == null)
                return false;

            var bondDec = bond as BondDecorator;
            if (bondDec == null)
                return bond.GetType() == type;

            return bondDec.TestHasType(type);
        }

        public static Tuple<string, string> GetBondElementNames(this IBond bond)
        {
            bond = bond ?? throw new ArgumentNullException(nameof(bond));

            var atom1 = bond.GetBondedAtom(null);
            var atom2 = bond.GetBondedAtom(atom1);

            return new Tuple<string, string>(atom1.Name, atom2.Name);
        }
    }
}
