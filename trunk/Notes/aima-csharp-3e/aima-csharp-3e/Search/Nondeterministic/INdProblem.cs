using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Agent;

namespace aima_csharp_3e.Search.Nondeterministic
{
    public interface INdProblem
    {
        IEnumerable<Tuple<IAction, IEnumerable<IState>>> GetSuccessor(IState state);
        IState InitState { get; }
        bool IsGoalTest(CondPlan plan);
        bool IsGoalTest(IState state);
        double GetPathCost(double costUpToState1, IState state1, IAction action, IState state2);
    }

    public abstract class NdProblemBase : INdProblem
    {
        public abstract IEnumerable<Tuple<IAction, IEnumerable<IState>>> GetSuccessor(IState state);

        public abstract IState InitState { get; }

        public bool IsGoalTest(CondPlan plan)
        {
            if (plan.ResultStates == null)
                return IsGoalTest(plan.CurrentPosition);
            foreach (var gNode in plan.ResultStates.Where(x => !plan.Next.CurrentPosition.Equals(x)))
            {
                if (!IsGoalTest(gNode))
                    return false;
            }
            return IsGoalTest(plan.Next);
        }

        public abstract bool IsGoalTest(IState state);

        public abstract double GetPathCost(double costUpToState1, IState state1, IAction action, IState state2);
    }
}
