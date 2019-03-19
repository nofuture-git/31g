using System;

namespace NoFuture.Rand.Core.Enums
{
    [Serializable]
    public enum Classification
    {
        /// <summary>
        /// Activity which must be done.
        /// </summary>
        Mandatory,

        /// <summary>
        /// Permits, forbids, and\or regulates all activity.
        /// </summary>
        Pervasive,

        /// <summary>
        /// Activity which must NOT be done.
        /// </summary>
        Prohibitory,

        /// <summary>
        /// Activity which may or may not be done.
        /// </summary>
        Permissive,
    }
}
