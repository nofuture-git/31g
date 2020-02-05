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
            return new SigmaBond(bond);
        }

        public static IBond AsPiBond(this IBond bond)
        {
            return new PiBond(bond);
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

            if(bond.Is(typeof(DoubleBond)))
                throw new ArgumentException($"{bond.GetBondElementNames()} " +
                                            $"is already declared as a {nameof(DoubleBond)}");
            if (bond.Is(typeof(TripleBond)))
                throw new ArgumentException($"{bond.GetBondElementNames()} " +
                                            $"is already declared as a {nameof(TripleBond)}");

            return new SingleBond(bond);
        }

        public static IBond AsDoubleBond(this IBond bond)
        {
            if (bond.Is(typeof(SingleBond)))
                throw new ArgumentException($"{bond.GetBondElementNames()} " +
                                            $"is already declared as a {nameof(SingleBond)}");
            if (bond.Is(typeof(TripleBond)))
                throw new ArgumentException($"{bond.GetBondElementNames()} " +
                                            $"is already declared as a {nameof(TripleBond)}");
            return new DoubleBond(bond);
        }

        public static IBond AsTripleBond(this IBond bond)
        {
            if (bond.Is(typeof(DoubleBond)))
                throw new ArgumentException($"{bond.GetBondElementNames()} " +
                                            $"is already declared as a {nameof(DoubleBond)}");
            if (bond.Is(typeof(SingleBond)))
                throw new ArgumentException($"{bond.GetBondElementNames()} " +
                                            $"is already declared as a {nameof(SingleBond)}");
            return new TripleBond(bond);
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
