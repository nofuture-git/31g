using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCDK;
using NCDK.Templates;
using Notes.Chemistry.Elements.PeriodicTable;

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
                var nfAtom = atom.ToNfAtom();
                if (nfAtom is Carbon || nfAtom is Hydrogen)
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
