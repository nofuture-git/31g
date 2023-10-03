using System;
using NoFuture.Law.Enums;

namespace NoFuture.Law
{
    /// <inheritdoc cref="IObviate" />
    /// <summary>
    /// Any type which could be given a name.
    /// </summary>
    /// <remarks>
    /// Latin for &quot;be called&quot;
    /// </remarks>
    public interface IVoca : IObviate
    {
        /// <summary>
        /// Convenience method to get or set the Legal name
        /// </summary>
        string Name { get; set; }

        int NamesCount { get; }

        /// <summary>
        /// Adds or replaces the name by the given pair
        /// </summary>
        /// <param name="k"></param>
        /// <param name="name"></param>
        void AddName(KindsOfNames k, string name);

        /// <summary>
        /// Gets the value for the given <see cref="KindsOfNames"/>
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        string GetName(KindsOfNames k);

        bool AnyNames(Predicate<KindsOfNames> filter);
        bool AnyNames(Predicate<string> filter);
        bool AnyNames(Func<KindsOfNames, string, bool> filter);
        bool AnyNames();

        int RemoveName(Predicate<KindsOfNames> filter);
        int RemoveName(Predicate<string> filter);
        int RemoveName(Func<KindsOfNames, string, bool> filter);

        /// <summary>
        /// Gets an array of all the <see cref="KindsOfNames"/> currently present/
        /// </summary>
        /// <returns></returns>
        KindsOfNames[] GetAllKindsOfNames();

        /// <summary>
        /// Helper method to quickly move data from one to another
        /// </summary>
        /// <param name="voca"></param>
        void CopyNamesFrom(IVoca voca);
    }
}