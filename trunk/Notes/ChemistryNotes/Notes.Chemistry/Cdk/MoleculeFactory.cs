using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCDK;
using NCDK.Default;
using NCDK.RingSearches;

namespace Notes.Chemistry.Cdk
{
    public static class MoleculeFactory
    {
        public static IAtomContainer Propane()
        {
            var propane = new AtomContainer();

            var carbon = new[]
            {
                new Atom(ChemicalElement.C),
                new Atom(ChemicalElement.C),
                new Atom(ChemicalElement.C)
            };

            for (var i = 0; i < carbon.Length; i++)
            {
                propane.Atoms.Add(carbon[i]);
                for (var j = 0; j < 3; j++)
                {
                    var hij = new Atom(ChemicalElement.H);
                    propane.Atoms.Add(hij);
                    propane.Bonds.Add(new Bond(carbon[i], hij, BondOrder.Single));
                }
            }

            propane.Bonds.Add(new Bond(carbon[0], carbon[1], BondOrder.Single));
            propane.Bonds.Add(new Bond(carbon[1], carbon[2], BondOrder.Single));

            propane.SetProperty(NCDK.CDKPropertyName.Title, nameof(propane));

            return propane;
        }

        public static IAtomContainer Ethylene()
        {
            var ethylene = new AtomContainer();

            var carbon = new[]
            {
                new Atom(ChemicalElement.C),
                new Atom(ChemicalElement.C),
            };

            for (var i = 0; i < carbon.Length; i++)
            {
                ethylene.Atoms.Add(carbon[i]);
                for (var j = 0; j < 2; j++)
                {
                    var hij = new Atom(ChemicalElement.H);
                    ethylene.Atoms.Add(hij);
                    ethylene.Bonds.Add(new Bond(carbon[i], hij, BondOrder.Single));
                }
            }

            ethylene.Bonds.Add(new Bond(carbon[0], carbon[1], BondOrder.Double));
            ethylene.SetProperty(NCDK.CDKPropertyName.Title, nameof(ethylene));

            return ethylene;
        }

        public static IAtomContainer Ethyne()
        {
            var ethyne = new AtomContainer();

            var carbon = new[]
            {
                new Atom(ChemicalElement.C),
                new Atom(ChemicalElement.C),
            };

            for (var i = 0; i < carbon.Length; i++)
            {
                var hi = new Atom(ChemicalElement.H);
                ethyne.Atoms.Add(hi);
                ethyne.Atoms.Add(carbon[i]);
                ethyne.Bonds.Add(new Bond(carbon[i], hi, BondOrder.Single));
            }

            ethyne.Bonds.Add(new Bond(carbon[0], carbon[1], BondOrder.Triple));
            ethyne.SetProperty(NCDK.CDKPropertyName.Title, nameof(ethyne));
            return ethyne;
        }

        public static IAtomContainer Cyclopentene()
        {
            var cyclopentene = new Ring(5, "C");
            cyclopentene.Bonds[3].Order = BondOrder.Double;
            cyclopentene.SetProperty(NCDK.CDKPropertyName.Title, nameof(cyclopentene));
            return cyclopentene;
        }

        /// <remarks>a.k.a. retinol</remarks>
        public static IAtomContainer VitaminA()
        {
            var vitaminA = new Ring(6, "C");

            var ring0 = vitaminA.Bonds[0].Begin;
            var ring1 = vitaminA.Bonds[1].Begin;
            var ring2 = vitaminA.Bonds[1].End;

            var carbon0plus0 = new Atom(ChemicalElement.C);
            var carbon0plus1 = new Atom(ChemicalElement.C);
            vitaminA.Atoms.Add(carbon0plus0);
            vitaminA.Atoms.Add(carbon0plus1);
            vitaminA.Bonds.Add(new Bond(ring2, carbon0plus0, BondOrder.Single));
            vitaminA.Bonds.Add(new Bond(ring2, carbon0plus1, BondOrder.Single));

            var carbon0plus = new Atom(ChemicalElement.C);
            vitaminA.Atoms.Add(carbon0plus);
            vitaminA.Bonds.Add(new Bond(ring0, carbon0plus, BondOrder.Single));

            vitaminA.Bonds[0].Order = BondOrder.Double;

            var carbons = new[]
            {
                new Atom(ChemicalElement.C), new Atom(ChemicalElement.C), new Atom(ChemicalElement.C),
                new Atom(ChemicalElement.C), new Atom(ChemicalElement.C), new Atom(ChemicalElement.C),
                new Atom(ChemicalElement.C), new Atom(ChemicalElement.C), new Atom(ChemicalElement.C)
            };

            foreach(var c in carbons)
                vitaminA.Atoms.Add(c);

            vitaminA.Bonds.Add(new Bond(ring1, carbons[0], BondOrder.Single));

            vitaminA.Bonds.Add(new Bond(carbons[0], carbons[1], BondOrder.Double));

            vitaminA.Bonds.Add(new Bond(carbons[1], carbons[2], BondOrder.Single));

            var carbon2plus = new Atom(ChemicalElement.C);
            vitaminA.Atoms.Add(carbon2plus);
            vitaminA.Bonds.Add(new Bond(carbons[2], carbon2plus, BondOrder.Single));
            vitaminA.Bonds.Add(new Bond(carbons[2], carbons[3], BondOrder.Double));

            vitaminA.Bonds.Add(new Bond(carbons[3], carbons[4], BondOrder.Single));

            vitaminA.Bonds.Add(new Bond(carbons[4], carbons[5], BondOrder.Double));

            vitaminA.Bonds.Add(new Bond(carbons[5], carbons[6], BondOrder.Single));

            var carbon6plus = new Atom(ChemicalElement.C);
            vitaminA.Atoms.Add(carbon6plus);
            vitaminA.Bonds.Add(new Bond(carbons[6], carbon6plus, BondOrder.Single));
            vitaminA.Bonds.Add(new Bond(carbons[6], carbons[7], BondOrder.Double));

            vitaminA.Bonds.Add(new Bond(carbons[7], carbons[8], BondOrder.Single));

            var oxy = new Atom(ChemicalElement.O);
            vitaminA.Atoms.Add(oxy);
            vitaminA.Bonds.Add(new Bond(carbons[8], oxy, BondOrder.Single));

            var tailH = new Atom(ChemicalElement.H);
            vitaminA.Atoms.Add(tailH);
            vitaminA.Bonds.Add(new Bond(oxy, tailH, BondOrder.Single));

            vitaminA.SetProperty(NCDK.CDKPropertyName.Title, nameof(vitaminA));
            return vitaminA;
        }
    }
}
