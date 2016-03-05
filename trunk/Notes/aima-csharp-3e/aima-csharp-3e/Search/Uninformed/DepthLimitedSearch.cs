using aima_csharp_3e.Search.Framework;
using aima_csharp_3e.zzWorkArounds;

namespace aima_csharp_3e.Search.Uninformed
{
    /// <summary>
    /// [Fig. 3.12]
    /// </summary>
    public class DepthLimitedSearch : DepthFirstSearch
    {
        protected readonly int _limit;

        public DepthLimitedSearch(IProblem problem, int limit = 50): base(problem)
        {
            _limit = limit;
        }

        public override Node Search()
        {
            return RecursiveDls(new Node(_myProblem.InitState), _myProblem, _limit);
        }

        private Node RecursiveDls(Node node, IProblem problem, int limit)
        {
            if (problem.IsGoalTest(node.State))
                return node;
            if(node.Depth == limit)
                return new DepthLimitReached(node.State);

            foreach (var successor in node.Expand(problem))
            {
                var result = RecursiveDls(successor, problem, limit);
                if(result != null)
                    return result;
            }
            
            return new DepthLimitReached(node.State);
        }
    }


}
