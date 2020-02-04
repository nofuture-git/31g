using System;
using System.Linq;
using Notes.Chemistry.Elements.Blocks;
using Notes.Chemistry.Elements.ElectronCfg.Shells;
using Notes.Chemistry.Elements.Groups;
using Notes.Chemistry.Elements.Periods;

namespace Notes.Chemistry.Elements.PeriodicTable
{
    public class Hydrogen : ElementBase, IOtherNonMetal, I01Group, I1Period, ISBlock
    {
        public override byte AtomicNumber => 1;
        public override string Symbol => "H";
        public override double AtomicMass => 1.00794;
        public override double Electronegativity => 2.2;
        public override Phase RoomTempPhase => Phase.gas;
    }
    public class Helium : ElementBase, INobleGas, I18Group, I1Period, IPBlock
    {
        public override byte AtomicNumber => 2;
        public override string Symbol => "He";
        public override double AtomicMass => 4.002602;
        public override double Electronegativity => 0;
        public override Phase RoomTempPhase => Phase.gas;
    }
    public class Lithium : ElementBase, IAlkalaiMetal, I01Group, I2Period, ISBlock
    {
        public override byte AtomicNumber => 3;
        public override string Symbol => "Li";
        public override double AtomicMass => 6.941;
        public override double Electronegativity => 0.98;
    }
    public class Beryllium : ElementBase, IAlkalineEarthMetal, I02Group, I2Period, ISBlock
    {
        public override byte AtomicNumber => 4;
        public override string Symbol => "Be";
        public override double AtomicMass => 9.012182;
        public override double Electronegativity => 1.57;
    }
    public class Boron : ElementBase, IMetalloids, I13Group, I2Period, IPBlock
    {
        public override byte AtomicNumber => 5;
        public override string Symbol => "B";
        public override double AtomicMass => 10.811;
        public override double Electronegativity => 2.04;
    }
    public class Carbon : ElementBase, IOtherNonMetal, I14Group, I2Period, IPBlock
    {
        public override byte AtomicNumber => 6;
        public override string Symbol => "C";
        public override double AtomicMass => 12.0107;
        public override double Electronegativity => 2.55;

        public void HybridizeOrbits(int hybridizedCount)
        {
            var lShell = Shells.FirstOrDefault(s => s is LShell) as LShell;

            lShell?.HybridizeOrbits(hybridizedCount);
        }
    }
    public class Nitrogen : ElementBase, IOtherNonMetal, I15Group, I2Period, IPBlock
    {
        public override byte AtomicNumber => 7;
        public override string Symbol => "N";
        public override double AtomicMass => 14.0067;
        public override double Electronegativity => 3.04;
        public override Phase RoomTempPhase => Phase.gas;
    }
    public class Oxygen : ElementBase, IOtherNonMetal, I16Group, I2Period, IPBlock
    {
        public override byte AtomicNumber => 8;
        public override string Symbol => "O";
        public override double AtomicMass => 15.9994;
        public override double Electronegativity => 3.44;
        public override Phase RoomTempPhase => Phase.gas;
    }
    public class Fluorine : ElementBase, IHalogens, I17Group, I2Period, IPBlock
    {
        public override byte AtomicNumber => 9;
        public override string Symbol => "F";
        public override double AtomicMass => 18.9984032;
        public override double Electronegativity => 3.98;
        public override Phase RoomTempPhase => Phase.gas;
    }
    public class Neon : ElementBase, INobleGas, I18Group, I2Period, IPBlock
    {
        public override byte AtomicNumber => 10;
        public override string Symbol => "Ne";
        public override double AtomicMass => 20.1797;
        public override double Electronegativity => 0;
        public override Phase RoomTempPhase => Phase.gas;
    }
    public class Sodium : ElementBase, IAlkalaiMetal, I01Group, I3Period, ISBlock
    {
        public override byte AtomicNumber => 11;
        public override string Symbol => "Na";
        public override double AtomicMass => 22.98976928;
        public override double Electronegativity => 0.93;
    }
    public class Magnesium : ElementBase, IAlkalineEarthMetal, I02Group, I3Period, ISBlock
    {
        public override byte AtomicNumber => 12;
        public override string Symbol => "Mg";
        public override double AtomicMass => 24.3050;
        public override double Electronegativity => 1.31;
    }
    public class Aluminum : ElementBase, IPostTransitionMetal, I13Group, I3Period, IPBlock
    {
        public override byte AtomicNumber => 13;
        public override string Symbol => "Al";
        public override double AtomicMass => 26.9815386;
        public override double Electronegativity => 1.61;
    }
    public class Silicon : ElementBase, IMetalloids, I14Group, I3Period, IPBlock
    {
        public override byte AtomicNumber => 14;
        public override string Symbol => "Si";
        public override double AtomicMass => 28.0855;
        public override double Electronegativity => 1.9;
    }
    public class Phosphorus : ElementBase, IOtherNonMetal, I15Group, I3Period, IPBlock
    {
        public override byte AtomicNumber => 15;
        public override string Symbol => "P";
        public override double AtomicMass => 30.973762;
        public override double Electronegativity => 2.19;
    }
    public class Sulfur : ElementBase, IOtherNonMetal, I16Group, I3Period, IPBlock
    {
        public override byte AtomicNumber => 16;
        public override string Symbol => "S";
        public override double AtomicMass => 32.065;
        public override double Electronegativity => 2.58;
    }
    public class Chlorine : ElementBase, IHalogens, I17Group, I3Period, IPBlock
    {
        public override byte AtomicNumber => 17;
        public override string Symbol => "Cl";
        public override double AtomicMass => 35.453;
        public override double Electronegativity => 3.16;
        public override Phase RoomTempPhase => Phase.gas;
    }
    public class Argon : ElementBase, INobleGas, I18Group, I3Period, IPBlock
    {
        public override byte AtomicNumber => 18;
        public override string Symbol => "Ar";
        public override double AtomicMass => 39.948;
        public override double Electronegativity => 0;
        public override Phase RoomTempPhase => Phase.gas;
    }
    public class Potassium : ElementBase, IAlkalaiMetal, I01Group, I4Period, ISBlock
    {
        public override byte AtomicNumber => 19;
        public override string Symbol => "K";
        public override double AtomicMass => 39.0983;
        public override double Electronegativity => 0.82;
    }
    public class Calcium : ElementBase, IAlkalineEarthMetal, I02Group, I4Period, ISBlock
    {
        public override byte AtomicNumber => 20;
        public override string Symbol => "Ca";
        public override double AtomicMass => 40.078;
        public override double Electronegativity => 1;
    }
    public class Scandium : ElementBase, ITransitionMetal, I03Group, I4Period, IDBlock
    {
        public override byte AtomicNumber => 21;
        public override string Symbol => "Sc";
        public override double AtomicMass => 44.955912;
        public override double Electronegativity => 1.36;
    }
    public class Titanium : ElementBase, ITransitionMetal, I04Group, I4Period, IDBlock
    {
        public override byte AtomicNumber => 22;
        public override string Symbol => "Ti";
        public override double AtomicMass => 47.867;
        public override double Electronegativity => 1.54;
    }
    public class Vanadium : ElementBase, ITransitionMetal, I05Group, I4Period, IDBlock
    {
        public override byte AtomicNumber => 23;
        public override string Symbol => "V";
        public override double AtomicMass => 50.9415;
        public override double Electronegativity => 1.63;
    }
    public class Chromium : ElementBase, ITransitionMetal, I06Group, I4Period, IDBlock
    {
        public override byte AtomicNumber => 24;
        public override string Symbol => "Cr";
        public override double AtomicMass => 51.9961;
        public override double Electronegativity => 1.66;
    }
    public class Manganese : ElementBase, ITransitionMetal, I07Group, I4Period, IDBlock
    {
        public override byte AtomicNumber => 25;
        public override string Symbol => "Mn";
        public override double AtomicMass => 54.938045;
        public override double Electronegativity => 1.55;
    }
    public class Iron : ElementBase, ITransitionMetal, I08Group, I4Period, IDBlock
    {
        public override byte AtomicNumber => 26;
        public override string Symbol => "Fe";
        public override double AtomicMass => 55.845;
        public override double Electronegativity => 1.83;
    }
    public class Cobalt : ElementBase, ITransitionMetal, I09Group, I4Period, IDBlock
    {
        public override byte AtomicNumber => 27;
        public override string Symbol => "Co";
        public override double AtomicMass => 58.933195;
        public override double Electronegativity => 1.88;
    }
    public class Nickel : ElementBase, ITransitionMetal, I10Group, I4Period, IDBlock
    {
        public override byte AtomicNumber => 28;
        public override string Symbol => "Ni";
        public override double AtomicMass => 58.6934;
        public override double Electronegativity => 1.91;
    }
    public class Copper : ElementBase, ITransitionMetal, I11Group, I4Period, IDBlock
    {
        public override byte AtomicNumber => 29;
        public override string Symbol => "Cu";
        public override double AtomicMass => 63.546;
        public override double Electronegativity => 1.9;
    }
    public class Zinc : ElementBase, ITransitionMetal, I12Group, I4Period, IDBlock
    {
        public override byte AtomicNumber => 30;
        public override string Symbol => "Zn";
        public override double AtomicMass => 65.38;
        public override double Electronegativity => 1.65;
    }
    public class Gallium : ElementBase, IPostTransitionMetal, I13Group, I4Period, IPBlock
    {
        public override byte AtomicNumber => 31;
        public override string Symbol => "Ga";
        public override double AtomicMass => 69.723;
        public override double Electronegativity => 1.81;
    }
    public class Germanium : ElementBase, IMetalloids, I14Group, I4Period, IPBlock
    {
        public override byte AtomicNumber => 32;
        public override string Symbol => "Ge";
        public override double AtomicMass => 72.64;
        public override double Electronegativity => 2.01;
    }
    public class Arsenic : ElementBase, IMetalloids, I15Group, I4Period, IPBlock
    {
        public override byte AtomicNumber => 33;
        public override string Symbol => "As";
        public override double AtomicMass => 74.92160;
        public override double Electronegativity => 2.18;
    }
    public class Selenium : ElementBase, IOtherNonMetal, I16Group, I4Period, IPBlock
    {
        public override byte AtomicNumber => 34;
        public override string Symbol => "Se";
        public override double AtomicMass => 78.96;
        public override double Electronegativity => 2.55;
    }
    public class Bromine : ElementBase, IHalogens, I17Group, I4Period, IPBlock
    {
        public override byte AtomicNumber => 35;
        public override string Symbol => "Br";
        public override double AtomicMass => 79.904;
        public override double Electronegativity => 2.96;
        public override Phase RoomTempPhase => Phase.liquid;
    }
    public class Krypton : ElementBase, INobleGas, I18Group, I4Period, IPBlock
    {
        public override byte AtomicNumber => 36;
        public override string Symbol => "Kr";
        public override double AtomicMass => 83.798;
        public override double Electronegativity => 0;
        public override Phase RoomTempPhase => Phase.gas;
    }
    public class Rubidium : ElementBase, IAlkalaiMetal, I01Group, I5Period, ISBlock
    {
        public override byte AtomicNumber => 37;
        public override string Symbol => "Rb";
        public override double AtomicMass => 85.4678;
        public override double Electronegativity => 0.82;
    }
    public class Strontium : ElementBase, IAlkalineEarthMetal, I02Group, I5Period, ISBlock
    {
        public override byte AtomicNumber => 38;
        public override string Symbol => "Sr";
        public override double AtomicMass => 87.62;
        public override double Electronegativity => 0.95;
    }
    public class Yttrium : ElementBase, ITransitionMetal, I03Group, I5Period, IDBlock
    {
        public override byte AtomicNumber => 39;
        public override string Symbol => "Y";
        public override double AtomicMass => 88.90585;
        public override double Electronegativity => 1.22;
    }
    public class Zirconium : ElementBase, ITransitionMetal, I04Group, I5Period, IDBlock
    {
        public override byte AtomicNumber => 40;
        public override string Symbol => "Zr";
        public override double AtomicMass => 91.224;
        public override double Electronegativity => 1.33;
    }
    public class Niobium : ElementBase, ITransitionMetal, I05Group, I5Period, IDBlock
    {
        public override byte AtomicNumber => 41;
        public override string Symbol => "Nb";
        public override double AtomicMass => 92.90638;
        public override double Electronegativity => 1.6;
    }
    public class Molybdenum : ElementBase, ITransitionMetal, I06Group, I5Period, IDBlock
    {
        public override byte AtomicNumber => 42;
        public override string Symbol => "Mo";
        public override double AtomicMass => 95.96;
        public override double Electronegativity => 2.16;
    }
    public class Technetium : ElementBase, ITransitionMetal, I07Group, I5Period, IDBlock
    {
        public override byte AtomicNumber => 43;
        public override string Symbol => "Tc";
        public override double AtomicMass => 98D;
        public override double Electronegativity => 1.9;
        public override bool IsRadioactive => true;
    }
    public class Ruthenium : ElementBase, ITransitionMetal, I08Group, I5Period, IDBlock
    {
        public override byte AtomicNumber => 44;
        public override string Symbol => "Ru";
        public override double AtomicMass => 101.07;
        public override double Electronegativity => 2.2;
    }
    public class Rhodium : ElementBase, ITransitionMetal, I09Group, I5Period, IDBlock
    {
        public override byte AtomicNumber => 45;
        public override string Symbol => "Rh";
        public override double AtomicMass => 102.90550;
        public override double Electronegativity => 2.28;
    }
    public class Palladium : ElementBase, ITransitionMetal, I10Group, I5Period, IDBlock
    {
        public override byte AtomicNumber => 46;
        public override string Symbol => "Pd";
        public override double AtomicMass => 106.42;
        public override double Electronegativity => 2.2;
    }
    public class Silver : ElementBase, ITransitionMetal, I11Group, I5Period, IDBlock
    {
        public override byte AtomicNumber => 47;
        public override string Symbol => "Ag";
        public override double AtomicMass => 107.8682;
        public override double Electronegativity => 1.93;
    }
    public class Cadmium : ElementBase, ITransitionMetal, I12Group, I5Period, IDBlock
    {
        public override byte AtomicNumber => 48;
        public override string Symbol => "Cd";
        public override double AtomicMass => 112.411;
        public override double Electronegativity => 1.69;
    }
    public class Indium : ElementBase, IPostTransitionMetal, I13Group, I5Period, IPBlock
    {
        public override byte AtomicNumber => 49;
        public override string Symbol => "In";
        public override double AtomicMass => 114.818;
        public override double Electronegativity => 1.78;
    }
    public class Tin : ElementBase, IPostTransitionMetal, I14Group, I5Period, IPBlock
    {
        public override byte AtomicNumber => 50;
        public override string Symbol => "Sn";
        public override double AtomicMass => 118.710;
        public override double Electronegativity => 1.96;
    }
    public class Antimony : ElementBase, IMetalloids, I15Group, I5Period, IPBlock
    {
        public override byte AtomicNumber => 51;
        public override string Symbol => "Sb";
        public override double AtomicMass => 121.760;
        public override double Electronegativity => 2.05;
    }
    public class Tellurium : ElementBase, IMetalloids, I16Group, I5Period, IPBlock
    {
        public override byte AtomicNumber => 52;
        public override string Symbol => "Te";
        public override double AtomicMass => 127.60;
        public override double Electronegativity => 2.1;
    }
    public class Iodine : ElementBase, IHalogens, I17Group, I5Period, IPBlock
    {
        public override byte AtomicNumber => 53;
        public override string Symbol => "I";
        public override double AtomicMass => 126.90447;
        public override double Electronegativity => 2.66;
    }
    public class Xenon : ElementBase, INobleGas, I18Group, I5Period, IPBlock
    {
        public override byte AtomicNumber => 54;
        public override string Symbol => "Xe";
        public override double AtomicMass => 131.293;
        public override double Electronegativity => 0;
        public override Phase RoomTempPhase => Phase.gas;
    }
    public class Cesium : ElementBase, IAlkalaiMetal, I01Group, I6Period, ISBlock
    {
        public override byte AtomicNumber => 55;
        public override string Symbol => "Cs";
        public override double AtomicMass => 132.9054519;
        public override double Electronegativity => 0.79;
    }
    public class Barium : ElementBase, IAlkalineEarthMetal, I02Group, I6Period, ISBlock
    {
        public override byte AtomicNumber => 56;
        public override string Symbol => "Ba";
        public override double AtomicMass => 137.327;
        public override double Electronegativity => 0.89;
    }
    public class Lanthanum : ElementBase, ILanthanide, I6Period, IFBlock
    {
        public override byte AtomicNumber => 57;
        public override string Symbol => "La";
        public override double AtomicMass => 138.90547;
        public override double Electronegativity => 1.1;
    }
    public class Cerium : ElementBase, ILanthanide, I6Period, IFBlock
    {
        public override byte AtomicNumber => 58;
        public override string Symbol => "Ce";
        public override double AtomicMass => 140.116;
        public override double Electronegativity => 1.12;
    }
    public class Praseodymium : ElementBase, ILanthanide, I6Period, IFBlock
    {
        public override byte AtomicNumber => 59;
        public override string Symbol => "Pr";
        public override double AtomicMass => 140.90765;
        public override double Electronegativity => 1.13;
    }
    public class Neodymium : ElementBase, ILanthanide, I6Period, IFBlock
    {
        public override byte AtomicNumber => 60;
        public override string Symbol => "Nd";
        public override double AtomicMass => 144.242;
        public override double Electronegativity => 1.14;
    }
    public class Promethium : ElementBase, ILanthanide, I6Period, IFBlock
    {
        public override byte AtomicNumber => 61;
        public override string Symbol => "Pm";
        public override double AtomicMass => 145D;
        public override double Electronegativity => 1.13;
    }
    public class Samarium : ElementBase, ILanthanide, I6Period, IFBlock
    {
        public override byte AtomicNumber => 62;
        public override string Symbol => "Sm";
        public override double AtomicMass => 150.36;
        public override double Electronegativity => 1.17;
    }
    public class Europium : ElementBase, ILanthanide, I6Period, IFBlock
    {
        public override byte AtomicNumber => 63;
        public override string Symbol => "Eu";
        public override double AtomicMass => 151.964;
        public override double Electronegativity => 1.2;
    }
    public class Gadolinium : ElementBase, ILanthanide, I6Period, IFBlock
    {
        public override byte AtomicNumber => 64;
        public override string Symbol => "Gd";
        public override double AtomicMass => 157.25;
        public override double Electronegativity => 1.2;
    }
    public class Terbium : ElementBase, ILanthanide, I6Period, IFBlock
    {
        public override byte AtomicNumber => 65;
        public override string Symbol => "Tb";
        public override double AtomicMass => 158.92535;
        public override double Electronegativity => 1.2;
    }
    public class Dysprosium : ElementBase, ILanthanide, I6Period, IFBlock
    {
        public override byte AtomicNumber => 66;
        public override string Symbol => "Dy";
        public override double AtomicMass => 162.500;
        public override double Electronegativity => 1.22;
    }
    public class Holmium : ElementBase, ILanthanide, I6Period, IFBlock
    {
        public override byte AtomicNumber => 67;
        public override string Symbol => "Ho";
        public override double AtomicMass => 164.93032;
        public override double Electronegativity => 1.23;
    }
    public class Erbium : ElementBase, ILanthanide, I6Period, IFBlock
    {
        public override byte AtomicNumber => 68;
        public override string Symbol => "Er";
        public override double AtomicMass => 167.259;
        public override double Electronegativity => 1.24;
    }
    public class Thulium : ElementBase, ILanthanide, I6Period, IFBlock
    {
        public override byte AtomicNumber => 69;
        public override string Symbol => "Tm";
        public override double AtomicMass => 168.93421;
        public override double Electronegativity => 1.25;
    }
    public class Ytterbium : ElementBase, ILanthanide, I6Period, IFBlock
    {
        public override byte AtomicNumber => 70;
        public override string Symbol => "Yb";
        public override double AtomicMass => 173.054;
        public override double Electronegativity => 1.1;
    }
    public class Lutetium : ElementBase, ILanthanide, I6Period, IDBlock
    {
        public override byte AtomicNumber => 71;
        public override string Symbol => "Lu";
        public override double AtomicMass => 174.9668;
        public override double Electronegativity => 1.27;
    }
    public class Hafnium : ElementBase, ITransitionMetal, I04Group, I6Period, IDBlock
    {
        public override byte AtomicNumber => 72;
        public override string Symbol => "Hf";
        public override double AtomicMass => 178.49;
        public override double Electronegativity => 1.3;
    }
    public class Tantalum : ElementBase, ITransitionMetal, I05Group, I6Period, IDBlock
    {
        public override byte AtomicNumber => 73;
        public override string Symbol => "Ta";
        public override double AtomicMass => 180.94788;
        public override double Electronegativity => 1.5;
    }
    public class Tungsten : ElementBase, ITransitionMetal, I06Group, I6Period, IDBlock
    {
        public override byte AtomicNumber => 74;
        public override string Symbol => "W";
        public override double AtomicMass => 183.84;
        public override double Electronegativity => 2.36;
    }
    public class Rhenium : ElementBase, ITransitionMetal, I07Group, I6Period, IDBlock
    {
        public override byte AtomicNumber => 75;
        public override string Symbol => "Re";
        public override double AtomicMass => 186.207;
        public override double Electronegativity => 1.9;
    }
    public class Osmium : ElementBase, ITransitionMetal, I08Group, I6Period, IDBlock
    {
        public override byte AtomicNumber => 76;
        public override string Symbol => "Os";
        public override double AtomicMass => 190.23;
        public override double Electronegativity => 2.2;
    }
    public class Iridium : ElementBase, ITransitionMetal, I09Group, I6Period, IDBlock
    {
        public override byte AtomicNumber => 77;
        public override string Symbol => "Ir";
        public override double AtomicMass => 192.217;
        public override double Electronegativity => 2.2;
    }
    public class Platinum : ElementBase, ITransitionMetal, I10Group, I6Period, IDBlock
    {
        public override byte AtomicNumber => 78;
        public override string Symbol => "Pt";
        public override double AtomicMass => 195.084;
        public override double Electronegativity => 2.28;
    }
    public class Gold : ElementBase, ITransitionMetal, I11Group, I6Period, IDBlock
    {
        public override byte AtomicNumber => 79;
        public override string Symbol => "Au";
        public override double AtomicMass => 196.966569;
        public override double Electronegativity => 2.54;
    }
    public class Mercury : ElementBase, ITransitionMetal, I12Group, I6Period, IDBlock
    {
        public override byte AtomicNumber => 80;
        public override string Symbol => "Hg";
        public override double AtomicMass => 200.59;
        public override double Electronegativity => 2;
        public override Phase RoomTempPhase => Phase.liquid;
    }
    public class Thallium : ElementBase, IPostTransitionMetal, I13Group, I6Period, IPBlock
    {
        public override byte AtomicNumber => 81;
        public override string Symbol => "Tl";
        public override double AtomicMass => 204.3833;
        public override double Electronegativity => 2.04;
    }
    public class Lead : ElementBase, IPostTransitionMetal, I14Group, I6Period, IPBlock
    {
        public override byte AtomicNumber => 82;
        public override string Symbol => "Pb";
        public override double AtomicMass => 207.2;
        public override double Electronegativity => 2.33;
    }
    public class Bismuth : ElementBase, IPostTransitionMetal, I15Group, I6Period, IPBlock
    {
        public override byte AtomicNumber => 83;
        public override string Symbol => "Bi";
        public override double AtomicMass => 208.98040;
        public override double Electronegativity => 2.02;
        public override bool IsRadioactive => true;
    }
    public class Polonium : ElementBase, IPostTransitionMetal, I16Group, I6Period, IPBlock
    {
        public override byte AtomicNumber => 84;
        public override string Symbol => "Po";
        public override double AtomicMass => 209D;
        public override double Electronegativity => 2;
        public override bool IsRadioactive => true;
    }
    public class Astatine : ElementBase, IHalogens, I17Group, I6Period, IPBlock
    {
        public override byte AtomicNumber => 85;
        public override string Symbol => "At";
        public override double AtomicMass => 210D;
        public override double Electronegativity => 2.2;
        public override bool IsRadioactive => true;
    }
    public class Radon : ElementBase, INobleGas, I18Group, I6Period, IPBlock
    {
        public override byte AtomicNumber => 86;
        public override string Symbol => "Rn";
        public override double AtomicMass => 222D;
        public override double Electronegativity => 0;
        public override Phase RoomTempPhase => Phase.gas;
        public override bool IsRadioactive => true;
    }
    public class Francium : ElementBase, IAlkalaiMetal, I01Group, I7Period, ISBlock
    {
        public override byte AtomicNumber => 87;
        public override string Symbol => "Fr";
        public override double AtomicMass => 223D;
        public override double Electronegativity => 0.7;
        public override bool IsRadioactive => true;
    }
    public class Radium : ElementBase, IAlkalineEarthMetal, I02Group, I7Period, ISBlock
    {
        public override byte AtomicNumber => 88;
        public override string Symbol => "Ra";
        public override double AtomicMass => 226D;
        public override double Electronegativity => 0.9;
        public override bool IsRadioactive => true;
    }
    public class Actinium : ElementBase, IActinides, I7Period, IFBlock
    {
        public override byte AtomicNumber => 89;
        public override string Symbol => "Ac";
        public override double AtomicMass => 227D;
        public override double Electronegativity => 1.1;
        public override bool IsRadioactive => true;
    }
    public class Thorium : ElementBase, IActinides, I7Period, IFBlock
    {
        public override byte AtomicNumber => 90;
        public override string Symbol => "Th";
        public override double AtomicMass => 232.03806;
        public override double Electronegativity => 1.3;
        public override bool IsRadioactive => true;
    }
    public class Protactinium : ElementBase, IActinides, I7Period, IFBlock
    {
        public override byte AtomicNumber => 91;
        public override string Symbol => "Pa";
        public override double AtomicMass => 231.03588;
        public override double Electronegativity => 1.5;
        public override bool IsRadioactive => true;
    }
    public class Uranium : ElementBase, IActinides, I7Period, IFBlock
    {
        public override byte AtomicNumber => 92;
        public override string Symbol => "U";
        public override double AtomicMass => 238.02891;
        public override double Electronegativity => 1.38;
        public override bool IsRadioactive => true;
    }
    public class Neptunium : SyntheticElementBase, IActinides, I7Period, IFBlock
    {
        public override byte AtomicNumber => 93;
        public override string Symbol => "Np";
        public override double AtomicMass => 237D;
        public override double Electronegativity => 1.36;
    }
    public class Plutonium : SyntheticElementBase, IActinides, I7Period, IFBlock
    {
        public override byte AtomicNumber => 94;
        public override string Symbol => "Pu";
        public override double AtomicMass => 244D;
        public override double Electronegativity => 1.28;
    }
    public class Americium : SyntheticElementBase, IActinides, I7Period, IFBlock
    {
        public override byte AtomicNumber => 95;
        public override string Symbol => "Am";
        public override double AtomicMass => 243D;
        public override double Electronegativity => 1.3;
    }
    public class Curium : SyntheticElementBase, IActinides, I7Period, IFBlock
    {
        public override byte AtomicNumber => 96;
        public override string Symbol => "Cm";
        public override double AtomicMass => 247D;
        public override double Electronegativity => 1.3;
    }
    public class Berkelium : SyntheticElementBase, IActinides, I7Period, IFBlock
    {
        public override byte AtomicNumber => 97;
        public override string Symbol => "Bk";
        public override double AtomicMass => 247D;
        public override double Electronegativity => 1.3;
    }
    public class Californium : SyntheticElementBase, IActinides, I7Period, IFBlock
    {
        public override byte AtomicNumber => 98;
        public override string Symbol => "Cf";
        public override double AtomicMass => 251D;
        public override double Electronegativity => 1.3;
    }
    public class Einsteinium : SyntheticElementBase, IActinides, I7Period, IFBlock
    {
        public override byte AtomicNumber => 99;
        public override string Symbol => "Es";
        public override double AtomicMass => 252D;
        public override double Electronegativity => 1.3;
    }
    public class Fermium : SyntheticElementBase, IActinides, I7Period, IFBlock
    {
        public override byte AtomicNumber => 100;
        public override string Symbol => "Fm";
        public override double AtomicMass => 257D;
        public override double Electronegativity => 1.3;
    }
    public class Mendelevium : SyntheticElementBase, IActinides, I7Period, IFBlock
    {
        public override byte AtomicNumber => 101;
        public override string Symbol => "Md";
        public override double AtomicMass => 258D;
        public override double Electronegativity => 1.3;
    }
    public class Nobelium : SyntheticElementBase, IActinides, I7Period, IFBlock
    {
        public override byte AtomicNumber => 102;
        public override string Symbol => "No";
        public override double AtomicMass => 259D;
        public override double Electronegativity => 1.3;
    }
    public class Lawrencium : SyntheticElementBase, IActinides, I7Period, IDBlock
    {
        public override byte AtomicNumber => 103;
        public override string Symbol => "Lr";
        public override double AtomicMass => 262D;
        public override double Electronegativity => 1.3;
    }

    public class Rutherfordium : SyntheticElementBase, ITransitionMetal, I04Group, I7Period, IDBlock
    {
        public override byte AtomicNumber => 104;
        public override string Symbol => "Rf";
        public override double AtomicMass => 267D;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
    public class Dubnium : SyntheticElementBase, ITransitionMetal, I05Group, I7Period, IDBlock
    {
        public override byte AtomicNumber => 105;
        public override string Symbol => "Db";
        public override double AtomicMass => 268D;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
    public class Seaborgium : SyntheticElementBase, ITransitionMetal, I06Group, I7Period, IDBlock
    {
        public override byte AtomicNumber => 106;
        public override string Symbol => "Sg";
        public override double AtomicMass => 271D;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
    public class Bohrium : SyntheticElementBase, ITransitionMetal, I07Group, I7Period, IDBlock
    {
        public override byte AtomicNumber => 107;
        public override string Symbol => "Bh";
        public override double AtomicMass => 272D;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
    public class Hassium : SyntheticElementBase, ITransitionMetal, I08Group, I7Period, IDBlock
    {
        public override byte AtomicNumber => 108;
        public override string Symbol => "Hs";
        public override double AtomicMass => 270D;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
    public class Meitnerium : SyntheticElementBase, IUnknownGroup, I09Group, I7Period, IDBlock
    {
        public override byte AtomicNumber => 109;
        public override string Symbol => "Mt";
        public override double AtomicMass => 276D;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
    public class Darmstadtium : ElementBase, IUnknownGroup, I10Group, I7Period,IDBlock
    {
        public override byte AtomicNumber => 110;
        public override string Symbol => "Ds";
        public override double AtomicMass => 281D;
        public override bool IsArtificial => true;
        public override bool IsRadioactive => true;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
    public class Roentgenium : SyntheticElementBase, IUnknownGroup, I11Group, I7Period, IDBlock
    {
        public override byte AtomicNumber => 111;
        public override string Symbol => "Rg";
        public override double AtomicMass => 280D;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
    public class Copernicium : SyntheticElementBase, ITransitionMetal, I12Group, I7Period, IDBlock
    {
        public override byte AtomicNumber => 112;
        public override string Symbol => "Cn";
        public override double AtomicMass => 285D;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
    public class Nihonium : SyntheticElementBase, IUnknownGroup, I13Group
    {
        public override byte AtomicNumber => 113;
        public override string Symbol => "Nh";
        public override double AtomicMass => 284D;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
    public class Flerovium : SyntheticElementBase, IUnknownGroup, I14Group, I7Period, IPBlock
    {
        public override byte AtomicNumber => 114;
        public override string Symbol => "Fl";
        public override double AtomicMass => 289D;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
    public class Moscovium : SyntheticElementBase, IUnknownGroup, I15Group, I7Period
    {
        public override byte AtomicNumber => 115;
        public override string Symbol => "Mc";
        public override double AtomicMass => 288D;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
    public class Livermorium : SyntheticElementBase, IUnknownGroup, I16Group, I7Period
    {
        public override byte AtomicNumber => 116;
        public override string Symbol => "Lv";
        public override double AtomicMass => 293D;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
    public class Tennessine : SyntheticElementBase, IUnknownGroup, I17Group, I7Period
    {
        public override byte AtomicNumber => 117;
        public override string Symbol => "Ts";
        public override double AtomicMass => 294D;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
    public class Oganesson : SyntheticElementBase, INobleGas, I18Group, I7Period
    {
        public override byte AtomicNumber => 118;
        public override string Symbol => "Og";
        public override double AtomicMass => 294D;
        public override Phase RoomTempPhase => Phase.gas;
        public override double Electronegativity => throw new NotImplementedException($"The electronegativity of {Name} is unknown.");
    }
}
