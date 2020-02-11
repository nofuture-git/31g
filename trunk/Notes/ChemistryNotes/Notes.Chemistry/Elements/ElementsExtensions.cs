using System.Collections.Generic;
using System.Linq;
using Notes.Chemistry.Elements.ElectronCfg.Orbitals;
using Notes.Chemistry.Elements.ElectronCfg.Shells;
using Notes.Chemistry.Elements.Groups;
using Notes.Chemistry.Elements.Periods;

namespace Notes.Chemistry.Elements
{
    /// <summary>
    /// https://egonw.github.io/cdkbook/cheminfo.html
    /// </summary>
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

            var spOrbital = lShell?.Orbitals.FirstOrDefault(o => o is sp_hybridizedOrbitalGroup) as sp_hybridizedOrbitalGroup;

            return spOrbital?.GetBondAngle();
        }

        public static string GetBondGeometry(this IElement atom)
        {
            var lShell = atom?.Shells.FirstOrDefault(s => s is LShell) as LShell;

            var spOrbital = lShell?.Orbitals.FirstOrDefault(o => o is sp_hybridizedOrbitalGroup) as sp_hybridizedOrbitalGroup;

            return spOrbital?.GetBondGeometry();
        }

        public static int GetIndex<T>(this ISet<T> set, T item)
        {
            if (set == null)
                return -1;
            if (!set.Contains(item))
                return -1;

            var array = set.ToArray();
            for (var i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(item) || object.ReferenceEquals(array[i], item))
                    return i;
            }

            return -1;
        }

        public static T GetItem<T>(this ISet<T> set, int index)
        {
            if (set == null)
                return default(T);

            if (index < 0)
                return default(T);

            if (index >= set.Count)
                return default(T);

            return set.ToArray()[index];
        }

        public static T GetNextItem<T>(this SortedSet<T> set, T afterThisOne)
        {
            if (set == null)
                return default(T);

            if (afterThisOne == null)
                return set.First();

            var index = set.GetIndex(afterThisOne);
            if (index < 0 || index >= set.Count)
                return default(T);

            return set.GetItem(index + 1);
        }

        public static T GetPreviousItem<T>(this SortedSet<T> set, T beforeThisOne)
        {
            if (set == null)
                return default(T);

            if (beforeThisOne == null)
                return default(T);

            var index = set.GetIndex(beforeThisOne);
            if (index <= 0 || index >= set.Count)
                return default(T);

            return set.GetItem(index - 1);
        }
    }
}
