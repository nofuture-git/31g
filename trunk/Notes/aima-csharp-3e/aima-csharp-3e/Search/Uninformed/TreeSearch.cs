using System.Linq;
using aima_csharp_3e.Search.Framework;
using aima_csharp_3e.zzWorkArounds;

namespace aima_csharp_3e.Search.Uninformed
{

    /// <summary>
    /// Search through the successors of a problem to find a goal.
    /// The argument fringe should be an empty queue.
    /// Don't worry about repeated paths to a state. [Fig. 3.8]
    /// </summary>
    public class TreeSearch : ISearch<Node>
    {
        protected readonly IProblem _myProblem;
        protected readonly PyCollection<Node> _fringe;

        public TreeSearch(IProblem p, PyCollection<Node> fringe)
        {
            _myProblem = p;
            _fringe = fringe;
        }

        public virtual Node Search()
        {
            _fringe.Append(new Node(_myProblem.InitState));
            while (_fringe.Any())
            {
                var node = _fringe.Pop();
                if (_myProblem.IsGoalTest(node.State))
                    return node;
                _fringe.Extend(node.Expand(_myProblem));
            }

            return null;
        }
    }

    /// <summary>
    /// Search the shallowest nodes in the search tree first. [p 74]
    /// </summary>
    public class BreadthFirstTreeSearch : TreeSearch
    {
        public BreadthFirstTreeSearch(IProblem p) : base(p, new PyQueue<Node>()) { }
    }

    /// <summary>
    /// Search the deepest nodes in the search tree first. [p 74]
    /// </summary>
    public class DepthFirstTreeSearch : TreeSearch
    {
        public DepthFirstTreeSearch(IProblem p) : base(p, new PyStack<Node>()) { }
    }
}
