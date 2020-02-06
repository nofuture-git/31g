using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly List<IBond> _bonds = new List<IBond>();

        public int? AddBond(IBond bond)
        {
            throw new NotImplementedException();
        }

        public int? RemoveBond(IBond bond)
        {
            throw new NotImplementedException();
        }
    }
}
