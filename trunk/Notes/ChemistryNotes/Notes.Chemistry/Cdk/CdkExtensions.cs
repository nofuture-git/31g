using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using NCDK;
using NCDK.Default;
using NCDK.Depict;
using NCDK.Renderers;
using NCDK.Smiles;
using Notes.Chemistry.Elements.PeriodicTable;

namespace Notes.Chemistry.Cdk
{
    public static class CdkExtensions
    {
        public static NCDK.IAtom ToNcdkAtom(this Elements.IElement element)
        {
            if (element == null)
                return null;

            return new Atom(element.Symbol);
        }

        public static Elements.IElement ToNfAtom(this NCDK.IAtom atom)
        {
            if (atom == null)
                return null;

            var symbol = atom.Symbol;

            switch (symbol)
            {
                case "H":
                    return new Hydrogen();
                case "He":
                    return new Helium();
                case "Li":
                    return new Lithium();
                case "Be":
                    return new Beryllium();
                case "B":
                    return new Boron();
                case "C":
                    return new Carbon();
                case "N":
                    return new Nitrogen();
                case "O":
                    return new Oxygen();
                case "F":
                    return new Fluorine();
                case "Ne":
                    return new Neon();
                case "Na":
                    return new Sodium();
                case "Mg":
                    return new Magnesium();
                case "Al":
                    return new Aluminum();
                case "Si":
                    return new Silicon();
                case "P":
                    return new Phosphorus();
                case "S":
                    return new Sulfur();
                case "Cl":
                    return new Chlorine();
                case "Ar":
                    return new Argon();
                case "K":
                    return new Potassium();
                case "Ca":
                    return new Calcium();
                case "Sc":
                    return new Scandium();
                case "Ti":
                    return new Titanium();
                case "V":
                    return new Vanadium();
                case "Cr":
                    return new Chromium();
                case "Mn":
                    return new Manganese();
                case "Fe":
                    return new Iron();
                case "Co":
                    return new Cobalt();
                case "Ni":
                    return new Nickel();
                case "Cu":
                    return new Copper();
                case "Zn":
                    return new Zinc();
                case "Ga":
                    return new Gallium();
                case "Ge":
                    return new Germanium();
                case "As":
                    return new Arsenic();
                case "Se":
                    return new Selenium();
                case "Br":
                    return new Bromine();
                case "Kr":
                    return new Krypton();
                case "Rb":
                    return new Rubidium();
                case "Sr":
                    return new Strontium();
                case "Y":
                    return new Yttrium();
                case "Zr":
                    return new Zirconium();
                case "Nb":
                    return new Niobium();
                case "Mo":
                    return new Molybdenum();
                case "Tc":
                    return new Technetium();
                case "Ru":
                    return new Ruthenium();
                case "Rh":
                    return new Rhodium();
                case "Pd":
                    return new Palladium();
                case "Ag":
                    return new Silver();
                case "Cd":
                    return new Cadmium();
                case "In":
                    return new Indium();
                case "Sn":
                    return new Tin();
                case "Sb":
                    return new Antimony();
                case "Te":
                    return new Tellurium();
                case "I":
                    return new Iodine();
                case "Xe":
                    return new Xenon();
                case "Cs":
                    return new Cesium();
                case "Ba":
                    return new Barium();
                case "La":
                    return new Lanthanum();
                case "Ce":
                    return new Cerium();
                case "Pr":
                    return new Praseodymium();
                case "Nd":
                    return new Neodymium();
                case "Pm":
                    return new Promethium();
                case "Sm":
                    return new Samarium();
                case "Eu":
                    return new Europium();
                case "Gd":
                    return new Gadolinium();
                case "Tb":
                    return new Terbium();
                case "Dy":
                    return new Dysprosium();
                case "Ho":
                    return new Holmium();
                case "Er":
                    return new Erbium();
                case "Tm":
                    return new Thulium();
                case "Yb":
                    return new Ytterbium();
                case "Lu":
                    return new Lutetium();
                case "Hf":
                    return new Hafnium();
                case "Ta":
                    return new Tantalum();
                case "W":
                    return new Tungsten();
                case "Re":
                    return new Rhenium();
                case "Os":
                    return new Osmium();
                case "Ir":
                    return new Iridium();
                case "Pt":
                    return new Platinum();
                case "Au":
                    return new Gold();
                case "Hg":
                    return new Mercury();
                case "Tl":
                    return new Thallium();
                case "Pb":
                    return new Lead();
                case "Bi":
                    return new Bismuth();
                case "Po":
                    return new Polonium();
                case "At":
                    return new Astatine();
                case "Rn":
                    return new Radon();
                case "Fr":
                    return new Francium();
                case "Ra":
                    return new Radium();
                case "Ac":
                    return new Actinium();
                case "Th":
                    return new Thorium();
                case "Pa":
                    return new Protactinium();
                case "U":
                    return new Uranium();
                case "Np":
                    return new Neptunium();
                case "Pu":
                    return new Plutonium();
                case "Am":
                    return new Americium();
                case "Cm":
                    return new Curium();
                case "Bk":
                    return new Berkelium();
                case "Cf":
                    return new Californium();
                case "Es":
                    return new Einsteinium();
                case "Fm":
                    return new Fermium();
                case "Md":
                    return new Mendelevium();
                case "No":
                    return new Nobelium();
                case "Lr":
                    return new Lawrencium();
                case "Rf":
                    return new Rutherfordium();
                case "Db":
                    return new Dubnium();
                case "Sg":
                    return new Seaborgium();
                case "Bh":
                    return new Bohrium();
                case "Hs":
                    return new Hassium();
                case "Mt":
                    return new Meitnerium();
                case "Ds":
                    return new Darmstadtium();
                case "Rg":
                    return new Roentgenium();
                case "Cn":
                    return new Copernicium();
                case "Nh":
                    return new Nihonium();
                case "Fl":
                    return new Flerovium();
                case "Mc":
                    return new Moscovium();
                case "Lv":
                    return new Livermorium();
                case "Ts":
                    return new Tennessine();
                case "Og":
                    return new Oganesson();
                default:
                    throw new NotImplementedException();
            }
        }

        public static IAtomContainer ConvertCanoicalSMILES(string canonicalSmiles)
        {
            if (string.IsNullOrWhiteSpace(canonicalSmiles))
            {
                return null;
            }
            var bldr = ChemObjectBuilder.Instance;
            var smipar = new SmilesParser(bldr);

            var mol = smipar.ParseSmiles(canonicalSmiles);

            return mol;
        }

        public static IAtomContainer AddAtomContainer(this IAtomContainer mol, IAtomContainer addThis)
        {
            if (mol == null)
                return null;

            if (addThis == null)
                return mol;

            foreach (var atom in addThis.Atoms)
            {
                if (mol.Atoms.Contains(atom))
                    continue;
                mol.Atoms.Add(atom);
            }

            foreach (var bond in addThis.Bonds)
            {
                if (mol.Bonds.Contains(bond))
                    continue;
                mol.Bonds.Add(bond);
            }

            return mol;
        }

        public static bool IsCarbonAtom(this IAtom atom)
        {
            if (atom == null)
                return false;
            return atom.Symbol == "C";
        }

        public static bool IsOxygenAtom(this IAtom atom)
        {
            if (atom == null)
                return false;
            return atom.Symbol == "O";
        }

        public static bool IsHydrogenAtom(this IAtom atom)
        {
            if (atom == null)
                return false;
            return atom.Symbol == "H";
        }

        public static bool IsCarbon2OxygenBond(this IBond bond)
        {
            return IsCarbonToThis(bond, IsOxygenAtom);
        }

        public static bool IsCarbon2HydrogenBond(this IBond bond)
        {
            return IsCarbonToThis(bond, IsHydrogenAtom);
        }

        public static bool IsCarbon2CarbonBond(this IBond bond)
        {
            return IsCarbonToThis(bond, IsCarbonAtom);
        }

        public static bool IsCarbon2NitrogenBond(this IBond bond)
        {
            return IsCarbonToThis(bond, atom => atom.Symbol == "N");
        }

        internal static bool IsCarbonToThis(IBond bond, Predicate<IAtom> other)
        {
            if (bond == null || other == null)
                return false;
            var combo0 = bond.Begin.IsCarbonAtom() && other(bond.End);
            var combo1 = other(bond.Begin) && bond.End.IsCarbonAtom();

            return combo0 || combo1;
        }

        /// <summary>
        /// https://github.com/cdk/cdk/wiki/Toolkit-Rosetta
        /// </summary>
        /// <returns>Full path to created .png file</returns>
        public static string DepictSMILES(string canonicalSmiles, string folder = null, int width = 300, int height = 350)
        {
            var mol = ConvertCanoicalSMILES(canonicalSmiles);

            if (mol == null)
                return null;

            return DepictMolecule(mol, folder, width, height);
        }

        /// <summary>
        /// https://github.com/cdk/cdk/wiki/Toolkit-Rosetta
        /// </summary>
        /// <returns>Full path to created .png file</returns>
        public static string DepictMolecule(this NCDK.IAtomContainer mol, string folder = null, int width = 300, int height = 350)
        {
            folder = folder ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                         @"NCDK\Images");

            width = width < 100 ? 300 : width;
            height = height < 100 ? 350 : height;

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var dptgen = new DepictionGenerator { Size = new Size(width, height), ShowMoleculeTitle = true };
            dptgen.SymbolVisibility = SymbolVisibility.All;
            var depiction = dptgen.Depict(mol);

            var canonicalSmiles = mol.GetProperty<string>(NCDK.CDKPropertyName.Title) ?? Path.GetRandomFileName();

            var filterName = new StringBuilder();
            foreach (var c in canonicalSmiles.ToCharArray())
            {
                if (Path.GetInvalidPathChars().Contains(c)
                    || Path.GetInvalidFileNameChars().Contains(c))
                {
                    continue;
                }
                filterName.Append(c);
            }

            filterName.Append(".png");
            var filename = Path.Combine(folder, filterName.ToString());
            
            depiction.WriteTo(filename);

            return filename;
        }

        public static double GetCountMolecules(this IAtomContainer mol, double grams)
        {
            if (mol == null || grams <= 0)
                return 0D;
            
            var atom2Count = new Dictionary<IAtom, int>();

            foreach (var atom in mol.Atoms)
            {
                var dictKey = atom2Count.Keys.FirstOrDefault(k => k.Symbol == atom.Symbol);

                if (dictKey == null)
                    atom2Count.Add(atom, 1);
                else
                    atom2Count[dictKey] += 1;
            }

            var molGramSum = 0D;
            foreach (var key in atom2Count.Keys)
            {
                molGramSum += key.ToNfAtom().AtomicMass * atom2Count[key];
            }

            var dk = grams * 1 / molGramSum * 6.0221409e+23;
            return dk;
        }
    }
}
