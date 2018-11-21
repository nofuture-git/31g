using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Opes.US;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Opes
{
    /// <summary>
    /// A base type on which Income, Expense, Assets, etc. is built.
    /// </summary>
    [Serializable]
    public abstract class WealthBase : IObviate
    {
        public const double DF_STD_DEV_PERCENT = 0.0885D;
        protected internal const int DF_ROUND_DECIMAL_PLACES = 5;
        protected internal IComparer<ITempore> Comparer { get; } = new TemporeComparer();

        #region properties

        public virtual NamedReceivable[] CurrentItems => GetCurrent(MyItems);

        public virtual Pecuniam Total => CurrentItems.Sum();

        /// <summary>
        /// The items which belong to this <see cref="Division"/>
        /// </summary>
        protected internal abstract List<NamedReceivable> MyItems { get; }

        /// <summary>
        /// A name on which a clear division is understood (e.g. Income, Balance)
        /// </summary>
        protected internal abstract string DivisionName { get; }

        #endregion

        #region methods
        public virtual IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            var itemData = new Dictionary<string, object>();

            foreach (var p in MyItems)
            {
                if (p.Value == Pecuniam.Zero)
                    continue;
                AddOrReplace(itemData, p.ToData(txtCase));
            }

            return itemData;
        }

        public abstract void AddItem(NamedReceivable item);

        public abstract List<string> GetGroupNames(string division = null);

        public abstract IVoca[] GetItemNames(string division = null);

        public virtual void AddItem(string name, string groupName, Pecuniam expectedValue, DateTime? atTime = null,
            TimeSpan? dueFrequency = null)
        {
            var amt = expectedValue ?? Pecuniam.Zero;
            var dt = atTime.GetValueOrDefault(DateTime.UtcNow);
            var tss = dueFrequency ?? Constants.TropicalYear;
            var p = new NamedReceivable(new VocaBase(name, DivisionName)) {DueFrequency = tss };
            if (amt.Amount < 0M)
                p.AddNegativeValue(dt, amt);
            else
                p.AddPositiveValue(dt, amt);
            AddItem(p);
        }

        public virtual void AddItem(string name, string groupName, double expectedValue,
            CurrencyAbbrev c = CurrencyAbbrev.USD, DateTime? atTime = null,
            TimeSpan? dueFrequency = null)
        {
            var amt = new Pecuniam(Convert.ToDecimal(expectedValue), c);
            AddItem(name, groupName, amt, atTime, dueFrequency);
        }

        public virtual NamedReceivable[] GetAt(DateTime? dt)
        {
            return GetAt(dt, MyItems);
        }

        /// <summary>
        /// Maps the <see cref="Division"/> groups names to a function which produces that group&apos;s item names and rate.
        /// </summary>
        /// <returns></returns>
        protected abstract Dictionary<string, Func<AmericanDomusOpesOptions, Dictionary<string, double>>> GetItems2Functions();

        /// <summary>
        /// Gets all the items of the given Opes type as random values based on the 
        /// options.
        /// </summary>
        /// <param name="options"></param>
        protected internal abstract void RandomizeAllItems(AmericanDomusOpesOptions options);

        /// <summary>
        /// Helper method to get only those on-going items from within <see cref="items"/>
        /// (i.e. items whose end date is null and whose expected value is not zero).
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        protected internal virtual NamedReceivable[] GetCurrent(List<NamedReceivable> items)
        {
            if (items == null)
                return null;
            var o = items.Where(x => x.Terminus == null).ToList();
            o.Sort(Comparer);
            return o.ToArray();
        }

        /// <summary>
        /// Helper method to reduce <see cref="items"/> down to only 
        /// those in range of <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        protected internal virtual NamedReceivable[] GetAt(DateTime? dt, List<NamedReceivable> items)
        {
            if (items == null)
                return null;
            var atDateItems = dt == null
                ? items.ToList()
                : items.Where(x => x.IsInRange(dt.Value)).ToList();
            atDateItems.Sort(Comparer);
            return atDateItems.ToArray();
        }

        /// <summary>
        /// Factory method to get a list of group-name to group-rate using 
        /// the given <see cref="options"/>
        /// </summary>
        /// <param name="options"></param>
        /// <returns>
        /// A set of item names to some percent where the sum of all the names is 1 (i.e. 100%).
        /// </returns>
        protected internal virtual List<Tuple<string, double>> GetGroupNames2Portions(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();

            var grpNames = GetGroupNames(DivisionName);

            return options.GetNames2Portions(grpNames.ToArray());
        }

        /// <summary>
        /// Factory method to get a list of item-name to item-rate using
        /// the given <see cref="options"/>
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="options"></param>
        /// <returns>
        /// A set of item names to some percent where the sum of all the names is 1 (i.e. 100%).
        /// </returns>
        protected internal virtual List<Tuple<string, double>> GetItemNames2Portions(string groupName, AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();

            //get all the item names we are targeting
            var itemNames = GetItemNames(DivisionName).Where(x =>
                String.Equals(x.GetName(KindsOfNames.Group), groupName, StringComparison.OrdinalIgnoreCase)).ToArray();

            return options.GetNames2Portions(itemNames.Select(k => k.Name).ToArray());
        }

        protected virtual Pecuniam CalcValue(Pecuniam pecuniam, double d)
        {
            pecuniam = pecuniam ?? Pecuniam.Zero;
            return Math.Round(pecuniam.ToDouble() * d, 2).ToPecuniam();
        }

        /// <summary>
        /// Gets January 1st date from negative <see cref="back"/> years from this year&apos;s January 1st
        /// </summary>
        /// <returns></returns>
        protected internal DateTime GetYearNeg(int back)
        {
            //current year is year 0
            var year0 = DateTime.Today.Year;

            var startYear0 = new DateTime(year0, 1, 1);
            return startYear0.AddYears(-1 * Math.Abs(back));
        }

        /// <summary>
        /// Breaks the whole span of time for this instance into yearly blocks.
        /// </summary>
        /// <returns></returns>
        protected internal List<Tuple<DateTime, DateTime?>> GetYearsInDates(DateTime startDate)
        {
            var stDt = startDate;
            if(stDt.Day!=1 || stDt.Month != 1)
                stDt = new DateTime(stDt.Year, 1, 1);

            var datesOut = new List<Tuple<DateTime, DateTime?>>();

            if (stDt.Year == DateTime.Today.Year)
            {
                datesOut.Add(new Tuple<DateTime, DateTime?>(stDt, null));
                return datesOut;
            }

            var endDt = new DateTime(stDt.Year, 12, 31);

            for (var i = 0; i <= DateTime.Today.Year - stDt.Year; i++)
            {
                if (stDt.Year + i >= DateTime.Today.Year)
                {
                    datesOut.Add(new Tuple<DateTime, DateTime?>(stDt.AddYears(i), null));
                    continue;
                }

                datesOut.Add(new Tuple<DateTime, DateTime?>(stDt.AddYears(i), endDt.AddYears(i)));
            }

            return datesOut;
        }

        /// <summary>
        /// The reusable\common parts of <see cref="RandomizeAllItems"/> 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual NamedReceivable[] GetItemsForRange(AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();

            var itemsout = new List<NamedReceivable>();

            var grp2Rates = GetGroupNames2Portions(options);

            foreach (var grp in grp2Rates)
            {
                itemsout.AddRange(GetItemsForRange(grp, options));
            }
            return itemsout.ToArray();
        }

        /// <summary>
        /// The reusable\common parts of <see cref="RandomizeAllItems"/> 
        /// </summary>
        /// <param name="grp2Rate"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual NamedReceivable[] GetItemsForRange(Tuple<string, double> grp2Rate, AmericanDomusOpesOptions options)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();

            var itemsout = new List<NamedReceivable>();

            var name2Op = GetItems2Functions();

            var grpName = grp2Rate.Item1;
            var grpRate = grp2Rate.Item2;

            var useFxMapping = name2Op.ContainsKey(grpName);

            var grpRates = useFxMapping
                ? name2Op[grpName](options)
                : GetItemNames2Portions(grpName, options)
                    .ToDictionary(k => k.Item1, k => k.Item2);

            foreach (var item in grpRates.Keys)
            {
                var p = GetNamedReceivableForItemAndGroup(item, grpName, options, grpRates[item] * grpRate);
                itemsout.Add(p);
            }

            return itemsout.ToArray();
        }

        /// <summary>
        /// While <see cref="GetItemsForRange(AmericanDomusOpesOptions)"/> deals with all the 
        /// items of this <see cref="DivisionName"/> this is concerned with one-item-at-a-time.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="grpName"></param>
        /// <param name="options"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        protected internal virtual NamedReceivable GetNamedReceivableForItemAndGroup(string itemName, string grpName, AmericanDomusOpesOptions options, double rate)
        {
            options = options ?? AmericanDomusOpesOptions.RandomOpesOptions();

            var calcValueR = CalcValue((options.SumTotal ?? 0).ToPecuniam(), rate);
            var p = NamedReceivable.RandomNamedReceivalbleWithHistoryToSum(itemName, grpName, calcValueR,
                options.DueFrequency, options.Inception, options.Terminus);

            return p;
        }

        /// <summary>
        /// Adds the entries in <see cref="b"/> into <see cref="a"/> or updates the existing entry in <see cref="a"/>
        /// with the value from <see cref="b"/>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        protected static void AddOrReplace(IDictionary<string, object> a, IDictionary<string, object> b)
        {
            a = a ?? new Dictionary<string, object>();
            b = b ?? new Dictionary<string, object>();

            foreach (var k in b.Keys)
            {
                if (a.ContainsKey(k))
                    a[k] = b[k];
                else
                    a.Add(k, b[k]);
            }
        }
        #endregion

    }
}
