using System;
using System.Collections.Generic;
using System.Linq;
using Notes.Chemistry.Elements.Bonds;

namespace Notes.Chemistry.Elements
{
    public interface IMolecule
    {
        int? AddBond(IBond bond);
        int? RemoveBond(IBond bond);
    }

    public class Molecule : IMolecule
    {
        private readonly HashSet<IBond> _bonds = new HashSet<IBond>();

        public int? AddBond(IBond bond)
        {
            if (bond == null)
                return null;

            return _bonds.Add(bond) ? 1 : 0;
        }

        public int? RemoveBond(IBond bond)
        {
            if (bond == null)
                return null;

            if (!_bonds.Contains(bond))
                return null;

            return _bonds.Remove(bond) ? 1 : 0;
        }

        public IEnumerable<IElement> GetAtoms()
        {
            var set = new HashSet<IElement>();
            foreach (var bond in _bonds)
            {
                var atom1 = bond.GetBondedAtom(null);
                var atom2 = bond.GetBondedAtom(atom1);

                set.Add(atom1);
                set.Add(atom2);
            }

            return set;
        }
    }
}
