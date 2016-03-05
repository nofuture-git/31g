using System.Collections.Generic;
using System.Linq;
using aima_csharp_3e.Agent;
using aima_csharp_3e.Search.Framework;
using aima_csharp_3e.zzWorkArounds;

namespace aima_csharp_3e.Search.Uninformed
{
    /// <summary>
    /// Search through the successors of a problem to find a goal.
    /// The argument fringe should be an empty queue.
    /// If two paths reach a state, only use the best one. [Fig. 3.18]
    /// </summary>
    public class GraphSearch : TreeSearch
    {
        private readonly List<IState> _exploredSet = new List<IState>();

        public GraphSearch(IProblem p, PyCollection<Node> fringe) : base(p, fringe) { }

        public override Node Search()
        {
            _exploredSet.Clear();
            _fringe.Append(new Node(_myProblem.InitState));
            while (_fringe.Any())
            {
                var node = _fringe.Pop();
                if (_myProblem.IsGoalTest(node.State))
                    return node;

                if (_exploredSet.Contains(node.State))
                    continue;
                _exploredSet.Add(node.State);
                _fringe.Extend(node.Expand(_myProblem));
            }
            return null;
        }
    }

    /// <summary>
    /// Search the shallowest nodes in the search tree first. [p 74]
    /// </summary>
    public class BreadthFirstGraph : GraphSearch
    {
        public BreadthFirstGraph(IProblem p) : base(p, new PyQueue<Node>()) { }
    }

    /// <summary>
    /// Search the deepest nodes in the search tree first. [p 74]
    /// </summary>
    public class DepthFirstSearch : GraphSearch
    {
        public DepthFirstSearch(IProblem p) : base(p, new PyStack<Node>()) { }
    }
}
