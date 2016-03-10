using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Agent;
using aima_csharp_3e.Search.Framework;

namespace aima_csharp_3e.Search.Nondeterministic
{
    public class AndOrGraphSearch : ISearch<CondPlan>
    {
        private readonly INdProblem _myProblem;

        public AndOrGraphSearch(INdProblem problem)
        {
            _myProblem = problem;
        }

        public CondPlan Search()
        {
            return OrSearch(_myProblem.InitState, new Stack<IState>());
        }

        public CondPlan OrSearch(IState state, Stack<IState> path)
        {
            if (_myProblem.IsGoalTest(state))
                return new CondPlan {CurrentPosition = state};

            if (path.Any(x => x.Equals(state)))
                return null;
            foreach (var a in _myProblem.GetSuccessor(state))
            {
                
                var action = a.Item1;
                var states = a.Item2.ToList();
                
                path.Push(state);

                var plan = AndSearch(states, path);
                if(plan != null)
                    return new CondPlan {CurrentPosition = state, Action = action, Next = plan, ResultStates = states};

            }
            return null;
        }

        public CondPlan AndSearch(IEnumerable<IState> states, Stack<IState> path)
        {

            CondPlan plan = null;
            foreach (var s in states)
            {
                plan = OrSearch(s, path);
                if (plan == null)
                    return null;
            }
            return plan;
        }
    }
}
