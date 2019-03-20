using System.Collections.Generic;

namespace NoFuture.Rand.Law
{
    /// <summary>
    /// I type to record the reasoning of any conclusion.
    /// </summary>
    public interface IRationale
    {
        IEnumerable<string> GetReasonEntries();
        void AddReasonEntry(string msg);
        void AddReasonEntryRange(IEnumerable<string> msgs);
        void ClearReasons();
    }
}
