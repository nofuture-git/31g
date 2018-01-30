using System;

namespace NoFuture.Rand.Exo.UsGov.Bls
{
    /// <summary>
    /// Disclaimer [BLS.gov cannot vouch for the data or analyses derived from these data after the data have been retrieved from BLS.gov.]
    /// </summary>
    public interface ISeries
    {
        Uri ApiLink { get; }
        string Prefix { get; }
    }
}
