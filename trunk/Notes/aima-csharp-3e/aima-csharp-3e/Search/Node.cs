using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Agent;
using aima_csharp_3e.Search.Framework;

namespace aima_csharp_3e.Search
{
    public class Node
    {
        #region fields
        private IState _state;
        private Node _parent;
        private IAction _action;
        private int _depth;
        private double _pathCost;
        #endregion

        #region ctors
        public Node(IState state, Node parent = null, IAction action = null, double pathCost = 0.0D)
        {
            _state = state;
            _parent = parent;
            _action = action;
            _depth = _parent == null ? 1 : _parent.Depth + 1;
            _pathCost = pathCost;
        }
        #endregion

        #region properties
        public IState State { get { return _state; } }
        public Node Parent { get { return _parent;} }
        public IAction Action { get { return _action;} }
        public int Depth { get { return _depth;} }
        public double PathCost { get { return _pathCost;} }
        #endregion

        #region methods

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public IEnumerable<Node> GetPath()
        {
            var result = new Stack<Node>();
            result.Push(this);
            var x = this;
            while (x.Parent != null)
            {
                result.Push(x.Parent);
                x = x.Parent;
            }
            return result.ToArray();
        }

        public IEnumerable<Node> Expand(IProblem problem)
        {
            var listOfTuples = problem.GetSuccessor(this.State);
            var nodesOut = new List<Node>();

            foreach (var t in listOfTuples)
            {
                var nNode = new Node(t.Item2, this, t.Item1,
                    problem.GetPathCost(this.PathCost, this.State, t.Item1, t.Item2));
                nodesOut.Add(nNode);
            }
            return nodesOut;
        }

        #endregion

    }

    public class NodeFx : IFx<Node>
    {
        private Func<Node, double> _myFunc;
        public NodeFx(Func<Node, double> fx)
        {
            _myFunc = fx;
        }
        public virtual Func<Node, double> Eval { get {return _myFunc;} }

        public static NodeFx operator +(NodeFx f1, IFx<Node> f2)
        {
            return new NodeFx (node => f1.Eval(node) + f2.Eval(node));
        }
    }

    public class DepthLimitReached : Node
    {

        public DepthLimitReached(IState state, Node parent = null, IAction action = null, double pathCost = 0) : base(state, parent, action, pathCost)
        {
        }
    }
}
