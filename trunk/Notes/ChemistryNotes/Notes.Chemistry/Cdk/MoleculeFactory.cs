using System;
using System.Collections.Generic;
using NCDK;
using NCDK.Default;

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

        [Aka("acetylene")]
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

        [Aka("retinol")]
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

        public static IAtomContainer Benzene()
        {
            var benzene = new Ring(6, "C");

            benzene.Bonds[0].Order = BondOrder.Double;
            benzene.Bonds[2].Order = BondOrder.Double;
            benzene.Bonds[4].Order = BondOrder.Double;

            benzene.SetProperty(NCDK.CDKPropertyName.Title, nameof(benzene));
            benzene.IsAromatic = true;
            return benzene;
        }

        /// <summary>
        /// see https://pubchem.ncbi.nlm.nih.gov/compound/5288826#section=Canonical-SMILES
        /// </summary>
        /// <returns></returns>
        public static IAtomContainer Morphine()
        {
            var morphine = CdkExtensions.ConvertCanoicalSMILES("CN1CCC23C4C1CC5=C2C(=C(C=C5)O)OC3C(C=C4)O");
            morphine.SetProperty(NCDK.CDKPropertyName.Title, nameof(morphine));
            return morphine;
        }

        /// <summary>
        /// see https://pubchem.ncbi.nlm.nih.gov/compound/6389
        /// </summary>
        public static IAtomContainer Trichlorofluoromethane()
        {
            var trichlorofluoromethane = new AtomContainer();

            var carbon = trichlorofluoromethane.AddAtom("C");
            for (var i = 0; i < 3; i++)
                trichlorofluoromethane.AddBond(carbon, trichlorofluoromethane.AddAtom("Cl"), BondOrder.Single);

            trichlorofluoromethane.AddBond(carbon, trichlorofluoromethane.AddAtom("F"), BondOrder.Single);

            trichlorofluoromethane.SetProperty(NCDK.CDKPropertyName.Title, nameof(trichlorofluoromethane));

            return trichlorofluoromethane;
        }

        [Aka("dichlorodifluoromethane", "freon 12")]
        public static IAtomContainer Refrigerant()
        {
            var refrigerant = new AtomContainer();

            var carbon = refrigerant.AddAtom("C");
            for (var i = 0; i < 2; i++)
                refrigerant.AddBond(carbon, refrigerant.AddAtom("Cl"), BondOrder.Single);

            for(var i = 0; i < 2; i++)
                refrigerant.AddBond(carbon, refrigerant.AddAtom("F"), BondOrder.Single);

            refrigerant.SetProperty(NCDK.CDKPropertyName.Title, nameof(refrigerant));

            return refrigerant;
        }

        [Aka("dichlorodiphenyl-trichloroethane")]
        public static IAtomContainer DDT()
        {
            var ddt = new AtomContainer();

            var benzeneLeft = Benzene();
            benzeneLeft.AddBond(benzeneLeft.Atoms[0], benzeneLeft.AddAtom("Cl"), BondOrder.Single);

            var benzeneRight = Benzene();
            benzeneRight.AddBond(benzeneRight.Atoms[0], benzeneRight.AddAtom("Cl"), BondOrder.Single);

            var carbon00 = ddt.AddAtom("C");
            benzeneLeft.Atoms.Add(carbon00);

            benzeneLeft.AddBond(benzeneLeft.Atoms[3], carbon00, BondOrder.Single);

            benzeneRight.Atoms.Add(carbon00);
            benzeneRight.AddBond(benzeneRight.Atoms[3], carbon00, BondOrder.Single);

            ddt.AddAtomContainer(benzeneLeft);
            ddt.AddAtomContainer(benzeneRight);

            var carbon01 = ddt.AddAtom("C");
            ddt.AddBond(carbon00, carbon01, BondOrder.Single);

            for (var i = 0; i < 3; i++)
                ddt.AddBond(carbon01, ddt.AddAtom("Cl"), BondOrder.Single);

            ddt.SetProperty(NCDK.CDKPropertyName.Title, "DDT");
            return ddt;
        }

        [Aka("ethyl alcohol", "beer", "wine", "liquor")]
        public static IAtomContainer Ethanol()
        {
            var ethanol = new AtomContainer();

            var carbon00 = ethanol.AddAtom("C");

            for (var i = 0; i < 3; i++)
                ethanol.AddBond(carbon00, ethanol.AddAtom("H"), BondOrder.Single);

            var carbon01 = ethanol.AddAtom("C");
            ethanol.AddBond(carbon00, carbon01, BondOrder.Single);

            for (var i = 0; i < 2; i++)
                ethanol.AddBond(carbon01, ethanol.AddAtom("H"), BondOrder.Single);

            var oxy = ethanol.AddAtom("O");
            ethanol.AddBond(carbon01, oxy, BondOrder.Single);
            ethanol.AddBond(oxy, ethanol.AddAtom("H"), BondOrder.Single);

            ethanol.SetProperty(NCDK.CDKPropertyName.Title, nameof(ethanol));

            return ethanol;
        }

        [Aka("isopropanol alcohol")]
        public static IAtomContainer RubbingAlcohol()
        {
            var rubbingAlcohol = new AtomContainer();

            var carbonTop = rubbingAlcohol.AddAtom("C");
            for (var i = 0; i < 3; i++)
                rubbingAlcohol.AddBond(carbonTop, rubbingAlcohol.AddAtom("H"), BondOrder.Single);

            var carbonBottom = rubbingAlcohol.AddAtom("C");
            for(var i = 0; i < 3; i++)
                rubbingAlcohol.AddBond(carbonBottom, rubbingAlcohol.AddAtom("H"), BondOrder.Single);

            var carbonCenter = rubbingAlcohol.AddAtom("C");

            rubbingAlcohol.AddBond(carbonCenter, carbonTop, BondOrder.Single);
            rubbingAlcohol.AddBond(carbonCenter, carbonBottom, BondOrder.Single);
            rubbingAlcohol.AddBond(carbonCenter, rubbingAlcohol.AddAtom("H"), BondOrder.Single);

            var oxy = rubbingAlcohol.AddAtom("O");
            rubbingAlcohol.AddBond(carbonCenter, oxy, BondOrder.Single);

            rubbingAlcohol.AddBond(oxy, rubbingAlcohol.AddAtom("H"), BondOrder.Single);

            rubbingAlcohol.SetProperty(NCDK.CDKPropertyName.Title, nameof(rubbingAlcohol));

            return rubbingAlcohol;
        }

        [Aka("ethylene glycol")]
        public static IAtomContainer Antifreeze()
        {
            var antifreeze = new AtomContainer();

            var oxy0 = antifreeze.AddAtom("O");
            antifreeze.AddBond(antifreeze.AddAtom("H"), oxy0, BondOrder.Single);

            var carbon0 = antifreeze.AddAtom("C");
            antifreeze.AddBond(oxy0, carbon0, BondOrder.Single);

            for (var i = 0; i < 2; i++)
                antifreeze.AddBond(carbon0, antifreeze.AddAtom("H"), BondOrder.Single);

            var carbon1 = antifreeze.AddAtom("C");
            antifreeze.AddBond(carbon0, carbon1, BondOrder.Single);

            for (var i = 0; i < 2; i++)
                antifreeze.AddBond(carbon1, antifreeze.AddAtom("H"), BondOrder.Single);

            var oxy1 = antifreeze.AddAtom("O");
            antifreeze.AddBond(carbon1, oxy1, BondOrder.Single);

            antifreeze.AddBond(oxy1, antifreeze.AddAtom("H"), BondOrder.Single);

            antifreeze.SetProperty(NCDK.CDKPropertyName.Title, nameof(antifreeze));

            return antifreeze;
        }

        public static IAtomContainer Water()
        {
            var water = new AtomContainer();

            var oxy = water.AddAtom("O");
            water.AddBond(oxy, water.AddAtom("H"), BondOrder.Single);
            water.AddBond(oxy, water.AddAtom("H"), BondOrder.Single);

            water.SetProperty(NCDK.CDKPropertyName.Title, nameof(water));

            return water;
        }

        /// <summary>
        /// See [https://pubchem.ncbi.nlm.nih.gov/compound/2723872#section=Canonical-SMILES]
        /// </summary>
        /// <returns></returns>
        public static IAtomContainer Fructose()
        {
            var fructose = CdkExtensions.ConvertCanoicalSMILES("C1C(C(C(C(O1)(CO)O)O)O)O");
            fructose.SetProperty(NCDK.CDKPropertyName.Title, nameof(fructose));
            return fructose;
        }

        /// <summary>
        /// See [https://pubchem.ncbi.nlm.nih.gov/compound/5793#section=Canonical-SMILES]
        /// </summary>
        /// <returns></returns>
        public static IAtomContainer Glucose()
        {
            var glucose = CdkExtensions.ConvertCanoicalSMILES("C(C1C(C(C(C(O1)O)O)O)O)O");
            glucose.SetProperty(NCDK.CDKPropertyName.Title, nameof(glucose));
            return glucose;
        }

        /// <summary>
        /// See [https://pubchem.ncbi.nlm.nih.gov/compound/5988#section=Canonical-SMILES]
        /// </summary>
        /// <returns></returns>
        [Aka("table sugar")]
        public static IAtomContainer Sucrose()
        {
            var sucrose = CdkExtensions.ConvertCanoicalSMILES("C(C1C(C(C(C(O1)OC2(C(C(C(O2)CO)O)O)CO)O)O)O)O");
            sucrose.SetProperty(NCDK.CDKPropertyName.Title, nameof(sucrose));
            return sucrose;
        }

        [Aka("ether")]
        public static IAtomContainer DiethylEther()
        {
            var diethylEther = new AtomContainer();

            var carbon0 = diethylEther.AddAtom("C");

            for (var i = 0; i < 3; i++)
                diethylEther.AddBond(carbon0, diethylEther.AddAtom("H"), BondOrder.Single);

            var carbon1 = diethylEther.AddAtom("C");
            diethylEther.AddBond(carbon0, carbon1, BondOrder.Single);

            for (var i = 0; i < 2; i++)
                diethylEther.AddBond(carbon1, diethylEther.AddAtom("H"), BondOrder.Single);

            var oxy = diethylEther.AddAtom("O");

            diethylEther.AddBond(carbon1, oxy, BondOrder.Single);

            var carbon2 = diethylEther.AddAtom("C");

            diethylEther.AddBond(oxy, carbon2, BondOrder.Single);

            for (var i = 0; i < 2; i++)
                diethylEther.AddBond(carbon2, diethylEther.AddAtom("H"), BondOrder.Single);

            var carbon3 = diethylEther.AddAtom("C");

            for (var i = 0; i < 3; i++)
                diethylEther.AddBond(carbon3, diethylEther.AddAtom("H"), BondOrder.Single);

            diethylEther.AddBond(carbon2, carbon3, BondOrder.Single);

            diethylEther.SetProperty(NCDK.CDKPropertyName.Title, nameof(diethylEther));

            return diethylEther;
        }

        public static IAtomContainer Benzaldehyde()
        {
            var benzaldehyde = Benzene();
            var carbon = benzaldehyde.AddAtom("C");

            benzaldehyde.AddBond(benzaldehyde.Atoms[0], carbon, BondOrder.Single);

            benzaldehyde.AddBond(carbon, benzaldehyde.AddAtom("O"), BondOrder.Double);
            benzaldehyde.AddBond(carbon, benzaldehyde.AddAtom("H"), BondOrder.Single);

            benzaldehyde.SetProperty(NCDK.CDKPropertyName.Title, nameof(benzaldehyde));

            return benzaldehyde;
        }

        public static IAtomContainer Acetone()
        {
            var acetone = new AtomContainer();

            var carbon0 = acetone.AddAtom("C");
            for (var i = 0; i < 3; i++)
                acetone.AddBond(carbon0, acetone.AddAtom("H"), BondOrder.Single);

            var carbon1 = acetone.AddAtom("C");

            acetone.AddBond(carbon0, carbon1, BondOrder.Single);

            acetone.AddBond(carbon1, acetone.AddAtom("O"), BondOrder.Double);

            var carbon2 = acetone.AddAtom("C");

            for (var i = 0; i < 3; i++)
                acetone.AddBond(carbon2, acetone.AddAtom("H"), BondOrder.Single);

            acetone.AddBond(carbon1, carbon2, BondOrder.Single);

            acetone.SetProperty(NCDK.CDKPropertyName.Title, nameof(acetone));

            return acetone;
        }

        [Aka("acetic acid")]
        public static IAtomContainer Vinegar()
        {
            var vinegar = new AtomContainer();

            var carbon0 = vinegar.AddAtom("C");
            for (var i = 0; i < 3; i++)
                vinegar.AddBond(carbon0, vinegar.AddAtom("H"), BondOrder.Single);

            var carbon1 = vinegar.AddAtom("C");
            vinegar.AddBond(carbon0, carbon1, BondOrder.Single);

            vinegar.AddBond(carbon1, vinegar.AddAtom("O"), BondOrder.Double);

            var oxy = vinegar.AddAtom("O");
            vinegar.AddBond(carbon1, oxy, BondOrder.Single);

            vinegar.AddBond(oxy, vinegar.AddAtom("H"), BondOrder.Single);

            vinegar.SetProperty(NCDK.CDKPropertyName.Title, nameof(vinegar));

            return vinegar;
        }

        public static IAtomContainer Glycine()
        {
            var glycine = new AtomContainer();

            var nitrogen = glycine.AddAtom("N");
            for (var i = 0; i < 2; i++)
                glycine.AddBond(nitrogen, glycine.AddAtom("H"), BondOrder.Single);

            var carbon0 = glycine.AddAtom("C");

            glycine.AddBond(nitrogen, carbon0, BondOrder.Single);
            for (var i = 0; i < 2; i++)
                glycine.AddBond(carbon0, glycine.AddAtom("H"), BondOrder.Single);

            var carbon1 = glycine.AddAtom("C");

            glycine.AddBond(carbon0, carbon1, BondOrder.Single);
            glycine.AddBond(carbon1, glycine.AddAtom("O"), BondOrder.Double);

            var oxy = glycine.AddAtom("O");
            glycine.AddBond(carbon1, oxy, BondOrder.Single);
            glycine.AddBond(oxy, glycine.AddAtom("H"), BondOrder.Single);

            glycine.SetProperty(NCDK.CDKPropertyName.Title, nameof(glycine));

            return glycine;
        }

        [Aka("propyl pentanoate")]
        public static IAtomContainer PineappleSmell()
        {
            var pineappleSmell = new AtomContainer();

            var carbons = new[]
            {
                pineappleSmell.AddAtom("C"), pineappleSmell.AddAtom("C"),
                pineappleSmell.AddAtom("C"), pineappleSmell.AddAtom("C"),
                pineappleSmell.AddAtom("C"), pineappleSmell.AddAtom("C"),
                pineappleSmell.AddAtom("C"), pineappleSmell.AddAtom("C"),
            };

            var oxygens = new[]
            {
                pineappleSmell.AddAtom("O"), pineappleSmell.AddAtom("O")
            };

            pineappleSmell.AddBond(carbons[0], carbons[1], BondOrder.Single);
            pineappleSmell.AddBond(carbons[1], carbons[2], BondOrder.Single);
            pineappleSmell.AddBond(carbons[2], carbons[3], BondOrder.Single);
            pineappleSmell.AddBond(carbons[3], carbons[4], BondOrder.Single);

            pineappleSmell.AddBond(carbons[4], oxygens[0], BondOrder.Double);
            pineappleSmell.AddBond(carbons[4], oxygens[1], BondOrder.Single);

            pineappleSmell.AddBond(oxygens[1], carbons[5], BondOrder.Single);
            pineappleSmell.AddBond(carbons[5], carbons[6], BondOrder.Single);
            pineappleSmell.AddBond(carbons[6], carbons[7], BondOrder.Single);

            pineappleSmell.SetProperty(NCDK.CDKPropertyName.Title, nameof(pineappleSmell));

            return pineappleSmell;
        }

        [Aka("ethyl butanoate")]
        public static IAtomContainer AppleSmell()
        {
            var appleSmell = new AtomContainer();

            var carbons = new[]
            {
                appleSmell.AddAtom("C"), appleSmell.AddAtom("C"),
                appleSmell.AddAtom("C"), appleSmell.AddAtom("C"),
                appleSmell.AddAtom("C"), appleSmell.AddAtom("C"),
            };

            var oxygens = new[]
            {
                appleSmell.AddAtom("O"), appleSmell.AddAtom("O")
            };

            appleSmell.AddBond(carbons[0], carbons[1], BondOrder.Single);
            appleSmell.AddBond(carbons[1], carbons[2], BondOrder.Single);
            appleSmell.AddBond(carbons[2], carbons[3], BondOrder.Single);

            appleSmell.AddBond(carbons[3], oxygens[0], BondOrder.Double);
            appleSmell.AddBond(carbons[3], oxygens[1], BondOrder.Single);

            appleSmell.AddBond(oxygens[1], carbons[4], BondOrder.Single);
            appleSmell.AddBond(carbons[4], carbons[5], BondOrder.Single);

            appleSmell.SetProperty(NCDK.CDKPropertyName.Title, nameof(appleSmell));

            return appleSmell;
        }

        public static IAtomContainer Penicillin()
        {
            var penicillin = CdkExtensions.ConvertCanoicalSMILES("CC1(C(N2C(S1)C(C2=O)NC(=O)CC3=CC=CC=C3)C(=O)O)C");
            penicillin.SetProperty(NCDK.CDKPropertyName.Title, nameof(penicillin));
            return penicillin;
        }

        public static IAtomContainer Acetonitrile()
        {
            var acetonitrile = new AtomContainer();

            var carbon0 = acetonitrile.AddAtom("C");
            for (var i = 0; i < 3; i++)
                acetonitrile.AddBond(carbon0, acetonitrile.AddAtom("H"), BondOrder.Single);

            var carbon1 = acetonitrile.AddAtom("C");
            acetonitrile.AddBond(carbon0, carbon1, BondOrder.Single);

            acetonitrile.AddBond(carbon1, acetonitrile.AddAtom("N"), BondOrder.Triple);

            acetonitrile.SetProperty(NCDK.CDKPropertyName.Title, nameof(acetonitrile));
            return acetonitrile;
        }
    }
}
