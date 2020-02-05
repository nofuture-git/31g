using System;

namespace Notes.Chemistry.Elements.Bonds.KindsOf
{
    public class PolarCovalent : BondDecorator
    {
        protected internal PolarCovalent(IBond bond) : base(bond)
        {
            if (Atom1.IsSameElement(Atom2))
                throw new InvalidOperationException($"Both atoms are {Atom1.Name} " +
                                                    $"which requires a {nameof(PurelyCovalent)} bond");

            if (Atom1.Electronegativity > Atom2.Electronegativity)
            {
                DeltaPlus = Atom2;
                DeltaMinus = Atom1;
            }
            else
            {
                DeltaPlus = Atom1;
                DeltaMinus = Atom2;
            }

            DipoleVectorSize = DeltaMinus.Electronegativity - DeltaMinus.Electronegativity;
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
