using System.Collections.Generic;
using NoFuture.Rand.Law.Enums;

namespace NoFuture.Rand.Law
{
    public interface IObviate
    {
        /// <summary>
        /// Reduces an object into a simple dictionary of name-value pairs
        /// </summary>
        /// <param name="txtCase"></param>
        /// <returns></returns>
        IDictionary<string, object> ToData(KindsOfTextCase txtCase);
    }
}
