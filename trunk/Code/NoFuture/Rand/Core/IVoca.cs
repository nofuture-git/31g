using System.Collections.Generic;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Any type which could be given a name.
    /// </summary>
    /// <remarks>
    /// Latin for 'be called'
    /// </remarks>
    public interface IVoca
    {
        /// <summary>
        /// Convenience method to get or set the Legal name
        /// </summary>
        string Name { get; set; }

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

        /// <summary>
        /// Asserts if there is an entry by the given <see cref="KindsOfNames"/>
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        bool AnyOfKind(KindsOfNames k);

        /// <summary>
        /// Asserts if there are any entries 
        /// which contain at least <see cref="k"/> in their combination.
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        bool AnyOfKindContaining(KindsOfNames k);

        /// <summary>
        /// Asserts if there is at least one entry by the given <see cref="name"/> value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool AnyOfNameAs(string name);

        /// <summary>
        /// Asserts if there is a unique entry by both kind and value
        /// </summary>
        /// <param name="k"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool AnyOfKindAndValue(KindsOfNames k, string name);

        /// <summary>
        /// Removes the item by the given <see cref="KindsOfNames"/>
        /// </summary>
        /// <param name="k"></param>
        /// <returns>
        /// True if something was removed, false if no match\no action
        /// </returns>
        bool RemoveNameByKind(KindsOfNames k);

        /// <summary>
        /// Removes all items whose value equals <see cref="name"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns>
        /// The count of items removed
        /// </returns>
        int RemoveNameByValue(string name);

        /// <summary>
        /// Removes a unique entry by both kind and value
        /// </summary>
        /// <param name="k"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool RemoveNameByKindAndValue(KindsOfNames k, string name);

        /// <summary>
        /// Gets the count of <see cref="KindsOfNames"/> to value pairs
        /// </summary>
        int GetCountOfNames();

        /// <summary>
        /// Gets an array of all the <see cref="KindsOfNames"/> currently present/
        /// </summary>
        /// <returns></returns>
        KindsOfNames[] GetAllKindsOfNames();

        /// <summary>
        /// Helper method to quickly move data from one to another
        /// </summary>
        /// <param name="voca"></param>
        void CopyFrom(IVoca voca);
    }
}