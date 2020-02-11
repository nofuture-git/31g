using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using NCDK;
using NCDK.Config;
using NCDK.Default;
using NCDK.Depict;
using NCDK.IO;
using NCDK.Renderers;
using NCDK.Smiles;
using NCDK.Tools;
using NCDK.Tools.Manipulator;

namespace Notes.Chemistry.CdkBook
{
    /// <summary>
    /// https://egonw.github.io/cdkbook/
    /// http://cdk.github.io/cdk/latest/docs/api/index.html
    /// </summary>
    public class CdkBookEd2dot3
    {
        public void Script3_1()
        {
            var atom = new Atom(6);
            var atom2 =  new Atom(ChemicalElement.C);
            

        }

        public void Script3_14()
        {
            var mol = new AtomContainer();
            mol.Atoms.Add(new Atom("C"));
            for (var i = 0; i < 4; i++)
                mol.Atoms.Add(new Atom("H"));
            for (var i = 0; i < 4; i++)
                mol.Bonds.Add(new Bond(mol.Atoms[0], mol.Atoms[i + 1]));
        }


        public void Script3_15()
        {
            var mol = new AtomContainer();
            
            mol.Atoms.Add(new Atom("C"));
            for (var i = 0; i < 4; i++)
                mol.Atoms.Add(new Atom("H"));
            for (var i = 0; i < 4; i++)
                mol.AddBond(mol.Atoms[0], mol.Atoms[i + 1], BondOrder.Single);


        }

        public static void Script8_3()
        {
            var parser = new SmilesParser();
            var hAdder = CDKHydrogenAdder.GetInstance();
            
            var methanol = parser.ParseSmiles("CO");
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(methanol);
            hAdder.AddImplicitHydrogens(methanol);
            AtomContainerManipulator.ConvertImplicitToExplicitHydrogens(methanol);
            
            var dimethoxymethane = parser.ParseSmiles("COC");
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(dimethoxymethane);
            hAdder.AddImplicitHydrogens(dimethoxymethane);
            AtomContainerManipulator.ConvertImplicitToExplicitHydrogens(dimethoxymethane);

            var water = parser.ParseSmiles("O");
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(water);
            hAdder.AddImplicitHydrogens(water);
            AtomContainerManipulator.ConvertImplicitToExplicitHydrogens(water);

            var reaction = new Reaction();
            reaction.Reactants.Add(methanol, 2.0D);
            reaction.Products.Add(dimethoxymethane);
            reaction.Products.Add(water);

            Console.WriteLine("Reactants:");

            foreach (var reactant in reaction.Reactants)
            {
                var formula = MolecularFormulaManipulator.GetMolecularFormula(reactant);
                Console.WriteLine(MolecularFormulaManipulator.GetString(formula));
            }

            Console.WriteLine("Products: ");
            foreach (var product in reaction.Products)
            {
                var formula = MolecularFormulaManipulator.GetMolecularFormula(product);
                Console.WriteLine(MolecularFormulaManipulator.GetString(formula));
            }
        }

        public static void Script10_1()
        {
            var builder = ChemObjectBuilder.Instance;
            var atom1 = builder.NewAtom(new Atom("C"));
            var atom2 = builder.NewAtom(new Atom("C"));
            var molecule = builder.NewAtomContainer();
            molecule.Atoms.Add(atom1);
            molecule.Atoms.Add(atom2);
            var bond = builder.NewBond(atom1, atom2, BondOrder.Single);
            molecule.Bonds.Add(bond);
        }

        public static void Script10_3()
        {
            var builder = ChemObjectBuilder.Instance;
            var molecule = builder.NewAtomContainer();
            molecule.Listeners.Add(new MyChemListener());
            var atom1 = builder.NewAtom("C");
            molecule.Atoms.Add(atom1);
            atom1.Symbol = "N";
        }

        public static void Script10_4()
        {
            System.Environment.SetEnvironmentVariable("cdk.debugging", "true");
            System.Environment.SetEnvironmentVariable("cdk.debug.stdout", "true");
            
        }

        public static void Script11_1()
        {
            var factory = new FormatFactory();
            var memStream = new MemoryStream(Encoding.UTF8.GetBytes("<molecule xmlns='http://www.xml-cml.org/schema' />"));
            var format = factory.GuessFormat(memStream);
            Console.WriteLine($"Format: {format.FormatName}");
        }

        public static void Script11_6()
        {
            
            System.Net.WebRequest.DefaultWebProxy = new System.Net.WebProxy(new Uri("http://qproxy.qdx.com:9090/"))
            {
                Credentials = System.Net.CredentialCache.DefaultNetworkCredentials
            };
            var webRequest = System.Net.WebRequest.CreateHttp("https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/cid/5282253/record/XML/?record_type=2d&response_type=save");
            webRequest.UseDefaultCredentials = true;
            webRequest.Proxy = new System.Net.WebProxy(new Uri("http://qproxy.qdx.com:9090/"));
            var webResponse = webRequest.GetResponse();
            var reader = new PCCompoundXMLReader(webResponse.GetResponseStream());
            var mol = reader.Read(new AtomContainer());
            Console.WriteLine($"Atom Count: {mol.Atoms.Count}");
            foreach (var keyValuePair in mol.GetProperties())
            {
                Console.WriteLine($"{keyValuePair.Key} {keyValuePair.Value}");
            }
            
        }

        public static void Script12_1()
        {
            var factory = AtomTypeFactory.GetInstance("NCDK.Dict.Data.cdk-atom-types.owl");
            var atomType = factory.GetAtomType("C.sp3");
            Console.WriteLine($"element: {atomType.Symbol}");
            Console.WriteLine($"formal charge: {atomType.FormalCharge}");
            Console.WriteLine($"hybridization: {atomType.Hybridization}");
            Console.WriteLine($"neighbors: {atomType.FormalNeighbourCount}");
            Console.WriteLine($"lone pairs: {atomType.GetProperty<int>(NCDK.CDKPropertyName.LonePairCount)}");
            Console.WriteLine($"pi bonds: {atomType.GetProperty<int>(NCDK.CDKPropertyName.PiBondCount)}");
        }

        /// <summary>
        /// https://github.com/cdk/cdk/wiki/Toolkit-Rosetta
        /// </summary>
        public static string DepictSMILES(string canonicalSmiles, string folder = null)
        {
            if (string.IsNullOrWhiteSpace(canonicalSmiles))
            {
                return null;
            }
            folder = folder ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                                            @"NCDK\Images");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var bldr = ChemObjectBuilder.Instance;
            var smipar = new SmilesParser(bldr);

            var mol = smipar.ParseSmiles(canonicalSmiles);

            var dptgen = new DepictionGenerator { Size = new Size(300, 350), ShowMoleculeTitle = true };
            dptgen.SymbolVisibility = SymbolVisibility.All;
            var depiction = dptgen.Depict(mol);

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

        public static void DepictExample01()
        {
            var bldr = ChemObjectBuilder.Instance;
            var smipar = new SmilesParser(bldr);

            var mol = smipar.ParseSmiles("CCC(=O)C");
            mol.SetProperty(NCDK.CDKPropertyName.Title, "butanone");

            var dptgen = new DepictionGenerator { Size = new Size(300, 350), ShowMoleculeTitle = true };
            dptgen.SymbolVisibility = SymbolVisibility.All;
            var depiction = dptgen.Depict(mol);
            depiction.WriteTo(@"C:\Projects\31g\trunk\Notes\ChemistryNotes\Notes.Chemistry\CdkBook\Images\butanone.png");
        }

        public static AtomContainer GetProgrammaticButanone()
        {
            var mol = new AtomContainer();
            var c00 = new Atom(ChemicalElement.C);
            mol.Atoms.Add(c00);
            for (var i = 0; i < 3; i++)
            {
                var h = new Atom(ChemicalElement.H);
                var bond = new Bond(c00, h, BondOrder.Single);
                mol.Bonds.Add(bond);
                mol.Atoms.Add(h);
            }

            var c01 = new Atom(ChemicalElement.C);
            mol.Atoms.Add(c01);
            for (var i = 0; i < 2; i++)
            {
                var h = new Atom(ChemicalElement.H);
                var bond = new Bond(c01, h, BondOrder.Single);
                mol.Bonds.Add(bond);
                mol.Atoms.Add(h);
            }

            mol.Bonds.Add(new Bond(c00, c01, BondOrder.Single));

            var c02 = new Atom(ChemicalElement.C);
            mol.Atoms.Add(c02);
            var o00 = new Atom(ChemicalElement.O);
            mol.Atoms.Add(o00);
            mol.Bonds.Add(new Bond(c02, o00, BondOrder.Double));

            mol.Bonds.Add(new Bond(c01, c02, BondOrder.Single));

            var c03 = new Atom(ChemicalElement.C);
            mol.Atoms.Add(c03);
            for (var i = 0; i < 3; i++)
            {
                var h = new Atom(ChemicalElement.H);
                var bond = new Bond(c03, h, BondOrder.Single);
                mol.Bonds.Add(bond);
                mol.Atoms.Add(h);
            }

            mol.Bonds.Add(new Bond(c02, c03, BondOrder.Single));


            mol.SetProperty(NCDK.CDKPropertyName.Title, "butanone");
            return mol;
        }

        public static void DepictExample02()
        {
            var butanone = CdkBookEd2dot3.GetProgrammaticButanone();
            var molSig = new NCDK.Signatures.MoleculeSignature(butanone);
            
            var memStream = new System.IO.MemoryStream();
            var smilesWriter = new NCDK.IO.SMILESWriter(memStream);
            smilesWriter.Write(butanone);
            var smilesSig = Encoding.UTF8.GetString(memStream.ToArray());

            var atomMatcher = NCDK.AtomTypes.CDKAtomTypeMatcher.GetInstance();
            foreach (var atom in butanone.Atoms)
            {
                var atomType = atomMatcher.FindMatchingAtomType(butanone, atom);
                if (atomType == null)
                    continue;
                NCDK.Tools.Manipulator.AtomTypeManipulator.Configure(atom, atomType);
            }
            NCDK.Tools.CDKHydrogenAdder.GetInstance().AddImplicitHydrogens(butanone);

            var dptgen = new DepictionGenerator { Size = new Size(300, 350), ShowMoleculeTitle = true };
            dptgen.SymbolVisibility = SymbolVisibility.All;
            var depiction = dptgen.Depict(butanone);

            depiction.WriteTo(@"C:\Projects\31g\trunk\Notes\ChemistryNotes\Notes.Chemistry\CdkBook\Images\butanone2.png");
        }

    }

    public class MyChemListener : IChemObjectListener
    {
        public void OnStateChanged(ChemObjectChangeEventArgs eventArgs)
        {
            Console.WriteLine("Event: " + eventArgs.Source);
        }
    }
}
