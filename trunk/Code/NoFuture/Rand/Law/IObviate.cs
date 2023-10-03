using System.Collections.Generic;
using NoFuture.Law.Enums;

namespace NoFuture.Law
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
