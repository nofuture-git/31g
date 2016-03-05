using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Search.Framework;

namespace aima_csharp_3e.Search.Local
{
    public class SimulatedAnnealing : ISearch<Node>
    {
        private readonly IProblem _myProblem;
        private readonly IFx<int> _mySchedule;
        private readonly Random _myRand = new Random(Convert.ToInt32(string.Format("{0:ffffff}", DateTime.Now)));

        public SimulatedAnnealing(IProblem problem, IFx<int> schedule)
        {
            _myProblem = problem;
            _mySchedule = schedule;
        }

        public Node Search()
        {
            var current = new Node(_myProblem.InitState);
            for (var i = 1; i < int.MaxValue; i++)
            {
                //this is like lowering the temp slowly from a high in metallurgy
                var t = _mySchedule.Eval(i);
                if(t.Equals(0.0D))
                    return current;
                //expansion of the node is determined by the problem
                var expanded = current.Expand(_myProblem).ToList();
                if(expanded.Count <= 0)
                    return current;
                //choose one at random
                var next = expanded[_myRand.Next(0, expanded.Count)];

                //sec. 4.1.2 "...picks a random move.  If the move improves the situation, it is always accepted.
                var deltaE = next.PathCost - current.PathCost;
                //            Otherwise, the algorithm accepts the move with sonic probability less than 1."
                if (deltaE > 0.0D || _myRand.NextDouble() <= Math.Exp(deltaE / t))
                    current = next;

                /* 
                  negative scores always have a lower prob. the further they are from
                  zero but everyones probablity is shrinking fast anyway.
                
                i       t           deltaE= -2    deltaE= -1.5    deltaE=-1
                1   19.11994964    0.900682215    0.924546354     0.949042789
                2   18.27862371    0.896356175    0.921213855     0.946760886
                3   17.47431823    0.891853249    0.917740821     0.944379823
                .     ...             ...           ...              ...
                97  0.254293104    0.000383972    0.002742995     0.019595213
                98  0.243103567    0.000267351    0.002090797     0.016350886
                99  0.232406397    0.000183075    0.00157388      0.013530518
            */
            }
            return current;
        }
    }
}
