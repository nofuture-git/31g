using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Agent;

namespace aima_csharp_3e.Search.Framework
{
    public interface IFx<T>
    {
        /// <summary>
        /// Will return '0' when when <see cref="T"/> resolves to the Goal
        /// </summary>
        /// <returns></returns>
        Func<T, double> Eval { get; }
    }

    public class FitnessFx : IFx<IState>
    {
        public FitnessFx(IProblem problem)
        {
            Eval = state => -1*problem.GetPathCost(0, state, null, state);
        }

        public Func<IState, double> Eval { get; }
    }
}
