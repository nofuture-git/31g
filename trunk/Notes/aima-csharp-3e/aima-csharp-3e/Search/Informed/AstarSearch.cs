using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Search.Framework;
using aima_csharp_3e.Search.Uninformed;
using aima_csharp_3e.zzWorkArounds;

namespace aima_csharp_3e.Search.Informed
{
    public class AstarSearch : GraphSearch
    {
        /// <summary>
        /// the cost to reach the node; gives the path cost from the start node to node (n)
        /// </summary>
        internal static readonly NodeFx gOfn = new NodeFx(node => node.PathCost);

        /// <summary>
        /// Sec. 3.5.2
        /// The most widely known form of best-first is called A* (pronounced "A-star")
        /// </summary>
        /// <param name="problem"></param>
        /// <param name="hOfn">
        /// the cost to get from the node to the goal;
        /// is the estimated cost of the cheapest path from (n) to the goal
        /// </param>
        public AstarSearch(IProblem problem, IFx<Node> hOfn)
            : base(problem, new PyPriorityQueue<Node>(gOfn + hOfn))
        {
        }

    }
}
