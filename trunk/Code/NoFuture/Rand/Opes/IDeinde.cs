using System;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Opes
{
    /// <summary>
    /// A wealth base containing methods common to all forms of Opes types
    /// Is Latin for &apos;then&apos;
    /// </summary>
    public interface IDeinde
    {
        void AddItem(NamedReceivable item);

        /// <summary>
        /// Convenience method to pass only the required 
        /// info to add a new <see cref="NamedReceivable"/> to the 
        /// instance&apos;s store
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groupName"></param>
        /// <param name="expectedValue"></param>
        /// <param name="interval"></param>
        void AddItem(string name, string groupName, Pecuniam expectedValue, Interval interval = Interval.Annually);

        /// <summary>
        /// Another convenience method to quickly add items manually
        /// </summary>
        /// <param name="name"></param>
        /// <param name="expectedValue"></param>
        /// <param name="interval"></param>
        /// <param name="c"></param>
        void AddItem(string name, double expectedValue, Interval interval = Interval.Annually,
            CurrencyAbbrev c = CurrencyAbbrev.USD);

        /// <summary>
        /// Convenience method to add an receivable from basic parts
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groupName"></param>
        /// <param name="expectedValue"></param>
        /// <param name="atTime"></param>
        /// <param name="dueFrequency"></param>
        void AddItem(string name, string groupName, Pecuniam expectedValue, DateTime? atTime = null,
            TimeSpan? dueFrequency = null);

        /// <summary>
        /// Another convenience method to quickly add items manually
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groupName"></param>
        /// <param name="expectedValue"></param>
        /// <param name="c"></param>
        /// <param name="atTime"></param>
        /// <param name="dueFrequency"></param>
        void AddItem(string name, string groupName, double expectedValue, CurrencyAbbrev c = CurrencyAbbrev.USD, DateTime? atTime = null,
            TimeSpan? dueFrequency = null);

    }
}
