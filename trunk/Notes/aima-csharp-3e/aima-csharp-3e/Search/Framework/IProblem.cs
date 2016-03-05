using System;
using System.Collections.Generic;
using aima_csharp_3e.Agent;

namespace aima_csharp_3e.Search.Framework
{
    public interface IProblem
    {
        IState InitState { get; }
        object Goal { get; }

        /// <summary>
        /// Given a state, return a sequence of (action, state) pairs reachable from this state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        IEnumerable<Tuple<IAction, IState>> GetSuccessor(IState state);
        bool IsGoalTest(IState state);

        /// <summary>
        /// Return the cost of a solution path that arrives at state2 from
        /// state1 via action, assuming cost c to get up to state1. If the problem
        /// is such that the path doesn't matter, this function will only look at
        /// state2.  If the path does matter, it will consider c and maybe state1
        /// and action. The default method costs 1 for every step in the path.
        /// </summary>
        /// <param name="costUpToState1"></param>
        /// <param name="state1"></param>
        /// <param name="action"></param>
        /// <param name="state2"></param>
        /// <returns></returns>
        double GetPathCost(double costUpToState1, IState state1, IAction action, IState state2);

        /// <summary>
        /// For optimization problems, each state has a value.  Hill-climbing
        /// and related algorithms try to maximize this value.
        /// </summary>
        /// <returns></returns>
        double GetValue();

    }
}
