using System;

namespace NoFuture.Rand.Law
{
    /// <summary>
    /// Test the logical relationship between the act and the charged\complained harm
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Example of being Factual Cause but not Proximate (Legal) Cause:
    /// Guy runs to catch train, doors closing, jumps on, loses balance,
    /// is assisted by train's guard, while being assited drops something
    /// wrapped in newspaper, dropped object falls onto tracks, turns out
    /// to be firecrakers, firecrakers explode, explosion causes shock wave,
    /// shock wave expands across train-plaform, shakes loose some heavy
    /// thing, heavy thing falls, hits somebody.
    /// Palsgraf v. Long Island R. Co., 248 N.Y. 339 (N.Y. 1928)
    /// ]]>
    /// </remarks>
    public interface IProximateCause<T> : ILegalConcept where T: IRationale
    {
        Predicate<T> IsDirectCause { get; set; }

        Predicate<T> IsForeseeable { get; set; }
    }
}
