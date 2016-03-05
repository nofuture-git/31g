using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Search;
using aima_csharp_3e.Search.Framework;

namespace aima_csharp_3e.Agent
{
    /// <summary>
    /// A simple problem-solving agent. It first formulates a goal and 
    /// a problem, searches for a sequence of actions that would solve
    /// the problem, and then executes the actions on at a time.
    /// When this is complete, it formulates another goal and starts over.
    /// </summary>
    public abstract class SimpleProblemSolvingAgent : IAgent
    {
        #region fields
        private IEnumerable<IAction> _seq = new List<IAction>();
        #endregion

        public bool IsAlive { get; set; }
        public IAction Exec(IPercept p)
        {
            var state = UpdateState(null, p);

            if (!_seq.Any())
            {
                var goal = FormulateGoal(state);
                var problem = FromulateProblem(goal);
                _seq = Search(problem);
            }
            var action = _seq.FirstOrDefault() ?? new NoOpAction();
            var seqLen = _seq.Count();
            _seq = _seq.Skip(1).Take(seqLen);
            return action;
        }

        protected abstract IState UpdateState(IState s, IPercept p);
        protected abstract IEnumerable<IAction> Search(IProblem problem);
        protected abstract Object FormulateGoal(IState s);
        protected abstract IProblem FromulateProblem(Object goal);
    }
}
