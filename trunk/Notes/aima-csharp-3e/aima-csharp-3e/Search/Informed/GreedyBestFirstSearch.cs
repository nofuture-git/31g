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
    public class GreedyBestFirstSearch : GraphSearch
    {
        public GreedyBestFirstSearch(IProblem p, IFx<Node> heuristicFx ) : base(p, new PyPriorityQueue<Node>(heuristicFx) ) { }
    }
}
