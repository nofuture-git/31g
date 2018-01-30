using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Domus.Opes
{
    /// <summary>
    /// A wealth base containing methods common to all forms of Opes types
    /// Is Latin for &apos;then&apos;
    /// </summary>
    public interface IDeinde
    {
        void AddItem(Pondus item);

        /// <summary>
        /// Convenience method to pass only the required 
        /// info to add a new <see cref="Pondus"/> to the 
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
    }
}
