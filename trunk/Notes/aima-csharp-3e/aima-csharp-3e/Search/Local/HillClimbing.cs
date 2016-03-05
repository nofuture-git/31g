using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Search.Framework;

namespace aima_csharp_3e.Search.Local
{
    public class HillClimbing : ISearch<Node>
    {
        private readonly IProblem _myProblem;
        private readonly IFx<Node> _calcValue;

        public HillClimbing(IProblem problem, IFx<Node> calcValue)
        {
            _myProblem = problem;
            _calcValue = calcValue;
        }

        public Node Search()
        {
            var current = new Node(_myProblem.InitState);
            for (;;) //ever
            {
                //the problem controls what this expands to based on this state
                var f = current.Expand(_myProblem);
                //hill climbing wants the highest value
                var dk = f.ToDictionary(node => _calcValue.Eval(node)).OrderBy(x => x.Key).First();
                if(dk.Value == null || dk.Key <= _calcValue.Eval(current))
                    return current;
                current = dk.Value;
            }
        }
    }
}
