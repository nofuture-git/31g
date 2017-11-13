using System;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// A grouping of bank used by the Federal Reserve
    /// </summary>
    [Serializable]
    public enum TypeOfBank
    {
        NationallyChartered,
        StateChartered,
        StateCharteredNonMember
    }
}