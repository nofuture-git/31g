using System;

namespace NoFuture.Shared
{
    public delegate void ProgressReportEvent(ProgressMessage message);

    /// <summary>
    /// Simple container class used to communicate progress, within an event,
    /// back to any subscribing assembly.
    /// </summary>
    [Serializable]
    public class ProgressMessage
    {
        public int ProgressCounter;
        public string Activity;
        public string Status;
        public string ProcName;
    }
}
