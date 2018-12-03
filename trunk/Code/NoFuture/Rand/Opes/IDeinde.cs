using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Opes
{
    /// <summary>
    /// A wealth base containing methods common to all forms of Opes types
    /// Is Latin for &apos;then&apos;
    /// </summary>
    public interface IDeinde : IObviate
    {
        /// <summary>
        /// Adds the <see cref="item"/> 
        /// </summary>
        /// <param name="item"></param>
        void AddItem(NamedTradeline item);

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

        /// <summary>
        /// Removes the item from the internal collection
        /// </summary>
        /// <param name="item"></param>
        void RemoveItem(NamedTradeline item);

        /// <summary>
        /// The items applicable to the current time
        /// </summary>
        NamedTradeline[] CurrentItems { get; }

        /// <summary>
        /// Gets the items applicable at the given time <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        NamedTradeline[] GetAt(DateTime? dt);

        /// <summary>
        /// Gets the sum total of the attached items at the current time
        /// </summary>
        Pecuniam Total { get; }
    }
}
