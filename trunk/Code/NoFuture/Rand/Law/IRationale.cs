using System.Collections.Generic;

namespace NoFuture.Rand.Law
{
    public interface IRationale
    {
        IEnumerable<string> GetReasonEntries();
        void AddReasonEntry(string msg);
        void AddReasonEntryRange(IEnumerable<string> msgs);
    }
}
