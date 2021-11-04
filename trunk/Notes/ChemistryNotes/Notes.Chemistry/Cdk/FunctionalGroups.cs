using System;
using System.Collections.Generic;
using System.Linq;
using NCDK;
using NCDK.RingSearches;
using Notes.Chemistry.Elements.Groups;

namespace Notes.Chemistry.Cdk
{
    public static class FunctionalGroups
    {
        public static bool IsHydrocarbon(this IAtomContainer mol)
        {
            if (mol == null)
                return false;

            foreach (var atom in mol.Atoms)
            {
                var symbol = atom.Symbol;
                if(symbol == "H" || symbol == "C")
                    continue;

                return false;
            }

            return true;
        }

        /// <summary>
        /// hydrocarbon with <see cref="BondOrder.Single"/> bound on a pair of carbon atoms
        /// </summary>
        public static bool IsAlkane(this IAtomContainer mol)
        {
            return TestIsAlk_x_ne(mol, BondOrder.Single);
        }

        /// <summary>
        /// hydrocarbon with <see cref="BondOrder.Double"/> bound on a pair of carbon atoms
        /// </summary>
        public static bool IsAlkene(this IAtomContainer mol)
        {
            return TestIsAlk_x_ne(mol, BondOrder.Double);
        }

        /// <summary>
        /// hydrocarbon with <see cref="BondOrder.Triple"/> bound on a pair of carbon atoms
        /// </summary>
        public static bool IsAlkyne(this IAtomContainer mol)
        {
            return TestIsAlk_x_ne(mol, BondOrder.Triple);
        }

        /// <summary>
        /// A compound with a nitrogen atom with two single-bonds to hydrogen atoms
        /// </summary>
        [Aka("amino")]
        public static bool IsPrimaryAmine(this IAtomContainer mol)
        {
            if (mol == null)
                return false;

            foreach (var atom in mol.Atoms)
            {
                if (!atom.IsNitrogenAtom())
                    continue;

                var nitrogenBonds = mol.GetConnectedBonds(atom)?.ToList();

                if (nitrogenBonds == null || !nitrogenBonds.Any())
                    continue;

                if (nitrogenBonds.Count != 3)
                    continue;

                if (nitrogenBonds.Count(b =>
                        b.Order == BondOrder.Single && (b.Begin.IsHydrogenAtom() || b.End.IsHydrogenAtom())) < 2)
                    continue;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Asserts the <see cref="mol"/> is only a simple aromatic ring.
        /// </summary>
        /// <remarks>
        /// https://cdk.github.io/cdk/1.5/docs/api/org/openscience/cdk/ringsearch/RingSearch.html
        /// </remarks>
        [Aka("aromatic")]
        public static bool IsArene(this IAtomContainer mol)
        {
            if (mol == null)
                return false;

            var ringSearch = new RingSearch(mol);

            //this is null for isolated and fused rings
            var cyclic = ringSearch.Cyclic();

            if (cyclic == null)
                return false;

            for (var i = 1; i < mol.Bonds.Count; i++)
            {
                var p0 = mol.Bonds[i - 1].Order == BondOrder.Single ^ mol.Bonds[i].Order == BondOrder.Single;
                var p1 = mol.Bonds[i - 1].Order == BondOrder.Double ^ mol.Bonds[i].Order == BondOrder.Double;
                if (p0 && p1)
                    continue;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Compounds that contain one or more halogens
        /// </summary>
        public static bool IsHalide(this IAtomContainer mol)
        {
            if (mol == null)
                return false;

            foreach (var atom in mol.Atoms)
            {
                if (!atom.IsCarbonAtom())
                    continue;

                var bonds = mol.GetConnectedBonds(atom)?.ToList();
                if (bonds == null || bonds.Count  != 4)
                    continue;

                foreach (var cBond in bonds)
                {
                    var connected = cBond.Atoms.FirstOrDefault(a => !a.IsCarbonAtom());
                    if (connected.ToNfAtom() is IHalogens)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Organic compounds ending with (...)-C-O-H 
        /// </summary>
        public static bool IsAlcohol(this IAtomContainer mol)
        {
            if (mol == null)
                return false;

            Func<IAtom, IAtom, bool> expr = (end0, end1) => end0.IsCarbonAtom() && end1.IsHydrogenAtom()
                                                            || end0.IsHydrogenAtom() && end1.IsCarbonAtom();

            return TestIsAlcoholEther(mol, expr);

        }

        /// <summary>
        /// Organic compound has an oxygen between two carbon atoms
        /// </summary>
        public static bool IsEther(this IAtomContainer mol)
        {
            Func<IAtom, IAtom, bool> expr = (end0, end1) => end0.IsCarbonAtom() && end1.IsCarbonAtom();
            return TestIsAlcoholEther(mol, expr);
        }

        /// <summary>
        /// Compound which has at least one carbon is double-bond to oxygen
        /// </summary>
        public static bool IsCarbonyl(this IAtomContainer mol)
        {
            if (mol == null)
                return false;
            foreach (var bond in mol.Bonds)
            {
                if (bond.IsCarbon2OxygenBond() && bond.Order == BondOrder.Double)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Compound which has at least one carbon is double-bond to oxygen and at least one single bond to hydrogen
        /// </summary>
        public static bool IsAldehyde(this IAtomContainer mol)
        {
            Predicate<IEnumerable<IBond>> expr = bonds => bonds.Any(CdkExtensions.IsCarbon2HydrogenBond);
            return TestCarbonToDblOxygen(mol, expr);
        }

        /// <summary>
        /// Compound which has at least one carbon is double-bond to oxygen
        /// while the rest of its bonds are single-bond to other carbon atoms
        /// </summary>
        public static bool IsKetone(this IAtomContainer mol)
        {
            Predicate<IEnumerable<IBond>> expr = bonds => bonds.All(CdkExtensions.IsCarbon2CarbonBond);
            return TestCarbonToDblOxygen(mol, expr);
        }

        /// <summary>
        /// Compound which is both <see cref="IsCarbonyl"/> and <see cref="IsAlcohol"/>
        /// </summary>
        public static bool IsCarboxylic(this IAtomContainer mol)
        {
            if (mol == null)
                return false;

            return mol.IsCarbonyl() && mol.IsAlcohol();
        }

        /// <summary>
        /// Compound which has a carbon double-bound to an oxygen and also
        /// single-bound to another oxygen.  In addition, this latter single-bond
        /// oxygen is itself connected on to some other carbon.
        /// </summary>
        /// <remarks> These are sweet smelling compounds </remarks>
        public static bool IsEster(this IAtomContainer mol)
        {
            if (mol == null)
                return false;

            Predicate<IBond> expr = carbonBond => carbonBond.IsCarbon2OxygenBond()
                                                  && carbonBond.Order == BondOrder.Single;

            return TestIsEsterAmide(mol, expr);
        }

        /// <summary>
        /// Same as <see cref="IsEster"/> except the latter single-bond is nitrogen instead of oxygen
        /// </summary>
        public static bool IsAmide(this IAtomContainer mol)
        {
            if (mol == null)
                return false;

            Predicate<IBond> expr = carbonBond => carbonBond.IsCarbon2NitrogenBond()
                                                  && carbonBond.Order == BondOrder.Single;

            return TestIsEsterAmide(mol, expr);
        }

        /// <summary>
        /// A compound with a carbon-nitrogen triple bond
        /// </summary>
        public static bool IsNitrile(this IAtomContainer mol)
        {
            if (mol == null)
                return false;

            foreach (var bond in mol.Bonds)
            {
                if (!bond.IsCarbon2NitrogenBond())
                    continue;
                if (bond.Order == BondOrder.Triple)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// A compound with one nitrogen bound to two oxygen
        /// </summary>
        public static bool IsNitro(this IAtomContainer mol)
        {
            if (mol == null)
                return false;

            foreach (var atom in mol.Atoms)
            {
                if(!atom.IsNitrogenAtom())
                    continue;

                var nitrogenBonds = mol.GetConnectedBonds(atom);

                if (nitrogenBonds.Count(b => b.IsNitrogen2OxygenBond()) == 2)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// A compound with a carbon-nitrogen single bond
        /// </summary>
        public static bool IsCyano(this IAtomContainer mol)
        {
            if (mol == null)
                return false;

            foreach (var bond in mol.Bonds)
            {
                if (bond.IsCarbon2NitrogenBond())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// A compound with one sulfur atom joined to three oxygen
        /// atoms total in which two of the three are double-bound
        /// while the third is single bound and, in addition, said
        /// third oxygen atom is, itself, bound to a hydrogen atom.
        /// </summary>
        public static bool IsSulphonic(this IAtomContainer mol)
        {
            if (mol == null)
                return false;

            foreach (var bond in mol.Bonds)
            {
                if (bond.Begin.Symbol != "S" && bond.End.Symbol != "S")
                    continue;

                var sulfur = bond.Begin.Symbol == "S" ? bond.Begin : bond.End;

                var sulfurBonds = mol.GetConnectedBonds(sulfur)?.ToList();
                if(sulfurBonds == null || !sulfurBonds.Any())
                    continue;

                if (sulfurBonds.Count < 4)
                    continue;

                if (sulfurBonds.Count(b => b.Order == BondOrder.Double && (b.Begin.IsOxygenAtom() || b.End.IsOxygenAtom()) ) != 2)
                    continue;

                var sulfur2OxygenSingle = sulfurBonds.FirstOrDefault(b =>
                    b.Order == BondOrder.Single && (b.Begin.IsOxygenAtom() || b.End.IsOxygenAtom()));

                if (sulfur2OxygenSingle == null)
                    continue;

                var singleBoundOxygen = sulfur2OxygenSingle.Begin.IsOxygenAtom()
                    ? sulfur2OxygenSingle.Begin
                    : sulfur2OxygenSingle.End;

                var singleBoundOxygenBonds = mol.GetConnectedBonds(singleBoundOxygen)?.ToList();
                if (singleBoundOxygenBonds == null || !singleBoundOxygenBonds.Any())
                    continue;

                if (singleBoundOxygenBonds.Count(b =>
                        b.Order == BondOrder.Single && (b.Begin.IsHydrogenAtom() || b.End.IsHydrogenAtom())) != 1)
                    continue;
                return true;

            }

            return false;
        }

        internal static bool TestIsEsterAmide(IAtomContainer mol, Predicate<IBond> expr)
        {
            if (mol == null || expr == null)
                return false;

            foreach (var bond in mol.Bonds)
            {
                if (!bond.IsCarbon2OxygenBond())
                    continue;

                if (bond.Order != BondOrder.Double)
                    continue;

                var carbon = bond.Begin.IsCarbonAtom() ? bond.Begin : bond.End;

                var carbonsBonds = mol.GetConnectedBonds(carbon)?.ToList();
                if (carbonsBonds == null || !carbonsBonds.Any())
                    continue;

                //remove our double-bond oxygen 
                carbonsBonds = carbonsBonds.Where(carbonBond => !bond.Equals(carbonBond)).ToList();

                //now, from this carbon find a single bond to another oxygen\ nitrogen
                var nextBondOnCarbon = carbonsBonds.FirstOrDefault(carbonBond => expr(carbonBond));

                if (nextBondOnCarbon == null)
                    continue;

                //move focus to this single-bond oxygen\ nitrogen
                var nextTargetAtom = nextBondOnCarbon.Begin.IsCarbonAtom() ? nextBondOnCarbon.End : nextBondOnCarbon.Begin;

                var nextTargetsBonds = mol.GetConnectedBonds(nextTargetAtom)?.ToList();
                if (nextTargetsBonds == null || !nextTargetsBonds.Any())
                    continue;

                //again, remove the bond that got us here
                var nextBond = nextTargetsBonds.FirstOrDefault(tBond => !nextBondOnCarbon.Equals(tBond));

                if (nextBond == null)
                    continue;

                //this oxygen\ nitrogen should then connect on to some carbon which continues the molecule 
                if (expr(nextBond))
                    return true;
            }

            return false;
        }

        internal static bool TestCarbonToDblOxygen(IAtomContainer mol, Predicate<IEnumerable<IBond>> expr)
        {
            if (mol == null || expr == null)
                return false;

            foreach (var bond in mol.Bonds)
            {
                if (!bond.IsCarbon2OxygenBond())
                    continue;

                if (bond.Order != BondOrder.Double)
                    continue;

                var carbon = bond.Begin.IsCarbonAtom() ? bond.Begin : bond.End;

                var connectedBonds = mol.GetConnectedBonds(carbon)?.ToList();
                if (connectedBonds == null || !connectedBonds.Any())
                    continue;
                //remove our double-bond oxygen 
                connectedBonds = connectedBonds.Where(carbonBond => !bond.Equals(carbonBond)).ToList();

                if (expr(connectedBonds))
                    return true;
            }

            return false;
        }

        internal static bool TestIsAlcoholEther(IAtomContainer mol, Func<IAtom, IAtom, bool> expr)
        {
            if (mol == null || expr == null)
                return false;

            foreach (var atom in mol.Atoms)
            {
                if (!atom.IsOxygenAtom())
                    continue;

                var bonds = mol.GetConnectedBonds(atom)?.ToList();
                if (bonds == null || bonds.Count != 2)
                    continue;

                var end0 = bonds[0].Atoms.FirstOrDefault(a => !a.IsOxygenAtom());
                var end1 = bonds[1].Atoms.FirstOrDefault(a => !a.IsOxygenAtom());

                if (expr(end0, end1))
                    return true;
            }

            return false;
        }

        internal static bool TestIsAlk_x_ne(IAtomContainer mol, BondOrder order)
        {
            if (mol == null)
                return false;

            if (!mol.IsHydrocarbon())
                return false;

            foreach (var bond in mol.Bonds)
            {
                if (bond.Begin.Symbol != "C" || bond.End.Symbol != "C")
                    continue;
                if (bond.Order != order)
                    continue;
                return true;
            }

            return false;
        }
    }
}
