using System.Linq;
using Notes.Chemistry.Elements.ElectronCfg.Orbitals;
using Notes.Chemistry.Elements.ElectronCfg.Shells;
using Notes.Chemistry.Elements.Groups;
using Notes.Chemistry.Elements.Periods;

namespace Notes.Chemistry.Elements
{
    public static class ElementsExtensions
    {
        public static double AvogadorosNumber => 6.022 * 10E+23;

        public static byte GetGroupNumber(this IGroup group)
        {
            switch (group)
            {
                case I01Group _:
                case IAlkalaiMetal _:
                    return 1;
                case I02Group _:
                case IAlkalineEarthMetal _:
                    return 2;
                case I03Group _:
                    return 3;
                case I04Group _:
                    return 4;
                case I05Group _:
                    return 5;
                case I06Group _:
                    return 6;
                case I07Group _:
                    return 7;
                case I08Group _:
                    return 8;
                case I09Group _:
                    return 9;
                case I10Group _:
                    return 10;
                case I11Group _:
                    return 11;
                case I12Group _:
                    return 12;
                case I13Group _:
                    return 13;
                case I14Group _:
                    return 14;
                case I15Group _:
                    return 15;
                case I16Group _:
                    return 16;
                case I17Group _:
                case IHalogens _:
                    return 17;
                case I18Group _:
                case INobleGas _:
                    return 18;
                default:
                    return 0;
            }
        }

        public static byte GetPeriodNumber(this IPeriod period)
        {
            switch (period)
            {
                case I1Period _:
                    return 1;
                case I2Period _:
                    return 2;
                case I3Period _:
                    return 3;
                case I4Period _:
                    return 4;
                case I5Period _:
                    return 5;
                case I6Period _:
                    return 6;
                case I7Period _:
                    return 7;
                default:
                    return 0;
            }
        }


        public static bool IsSameElement(this IElement atom1, IElement atom2)
        {
            return atom1?.GetType() == atom2?.GetType();
        }

        public static double? GetBondAngle(this IElement atom)
        {
            var lShell = atom?.Shells.FirstOrDefault(s => s is LShell) as LShell;

            var spOrbital = lShell?.Orbits.FirstOrDefault(o => o is sp_hybridizedOrbitals) as sp_hybridizedOrbitals;

            return spOrbital?.GetBondAngle();
        }

        public static string GetBondGeometry(this IElement atom)
        {
            var lShell = atom?.Shells.FirstOrDefault(s => s is LShell) as LShell;

            var spOrbital = lShell?.Orbits.FirstOrDefault(o => o is sp_hybridizedOrbitals) as sp_hybridizedOrbitals;

            return spOrbital?.GetBondGeometry();
        }
    }

    
}
