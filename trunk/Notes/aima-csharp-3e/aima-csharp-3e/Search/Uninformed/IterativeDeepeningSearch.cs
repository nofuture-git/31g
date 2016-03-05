using System;
using aima_csharp_3e.Search.Framework;

namespace aima_csharp_3e.Search.Uninformed
{
    /// <summary>
    /// [Fig. 3.13]
    /// </summary>
    public class IterativeDeepeningSearch : DepthLimitedSearch
    {
        public IterativeDeepeningSearch(IProblem problem) : base(problem, int.MaxValue) { }

        public override Node Search()
        {
            for (var i = 1; i < _limit - 1; i++)
            {
                var result = base.Search();
                if (!(result is DepthLimitReached))
                    return result;
            }
            throw new IndexOutOfRangeException("Reached max Int32 value and no Node was found.");
        }
    }
}
