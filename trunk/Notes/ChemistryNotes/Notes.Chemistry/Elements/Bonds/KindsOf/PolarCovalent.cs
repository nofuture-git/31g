using System;

namespace Notes.Chemistry.Elements.Bonds.KindsOf
{
    /// <summary>
    /// The covalent bond where electronegativity difference is
    /// small (between 0 and 2) and the electrons are shared but not equally
    /// </summary>
    public class PolarCovalent : BondDecorator
    {
        protected internal PolarCovalent(IBond bond) : base(bond)
        {
            var atom1 = GetBondedAtom(null);
            var atom2 = GetBondedAtom(atom1);
            if (atom1.IsSameElement(atom2))
                throw new InvalidOperationException($"Both atoms are {atom1.Name} " +
                                                    $"which requires a {nameof(PurelyCovalent)} bond");

            //dipole moment: the separation of charge in the bonds formation
            if (atom1.Electronegativity > atom2.Electronegativity)
            {
                DeltaPlus = atom2;
                DeltaMinus = atom1;
            }
            else
            {
                DeltaPlus = atom1;
                DeltaMinus = atom2;
            }

            DipoleVectorSize = DeltaMinus.Electronegativity - DeltaPlus.Electronegativity;
        }

        /// <summary>
        /// the difference in electronegativity 
        /// </summary>
        public double DipoleVectorSize { get; }

        /// <summary>
        /// The partially positive atom - the atom with the lower electronegativity
        /// </summary>
        public IElement DeltaPlus { get; }

        /// <summary>
        /// The partially negative atom - the atom with the higher electronegativity
        /// </summary>
        public IElement DeltaMinus { get; }
    }
}
