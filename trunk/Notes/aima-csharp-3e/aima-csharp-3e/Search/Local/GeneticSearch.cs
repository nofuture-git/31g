using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Agent;
using aima_csharp_3e.Search.Framework;

namespace aima_csharp_3e.Search.Local
{
    public class GeneticSearch : ISearch<IState>
    {
        #region fields
        private readonly IProblem _myProblem;
        private readonly Random _myRand = new Random(Convert.ToInt32(string.Format("{0:ffffff}", DateTime.Now)));
        private List<Tuple<double, IState>> _wtFitness2State = new List<Tuple<double, IState>>();
        private int _numberOfGenerations = 1000;
        private readonly Func<IState, IState> _mutationFx;
        private readonly Func<IState, IState, IState> _reproductionFx;
        #endregion

        #region ctors
        /// <summary>
        /// Sec. 4.1.4
        /// </summary>
        /// <param name="problem"></param>
        /// <param name="reproductionFunc">
        /// We make the calling assembly define how the two states produce one new one.
        /// </param>
        /// <param name="mutationFunc">
        /// We make the calling assembly define how and what a mutation of <see cref="IState"/> is.
        /// </param>
        /// <remarks>
        /// The <see cref="problem"/> is only used to resolve all the possiable states, the <see cref="IAction"/> are 
        /// disgarded and the results of the call to <see cref="IProblem.GetSuccessor"/> is returning the whole population.
        /// </remarks>
        public GeneticSearch(IProblem problem, Func<IState, IState, IState> reproductionFunc, Func<IState, IState> mutationFunc )
        {
            _myProblem = problem;
            FitnessFunction = new FitnessFx(problem);
            _mutationFx = mutationFunc;
            _reproductionFx = reproductionFunc;

            CalcWtFitness2State(problem.GetSuccessor(_myProblem.InitState).Select(x => x.Item2).ToList());
        }
        #endregion

        /// <summary>
        /// Allow calling assembly to define this, the default is 1000.
        /// </summary>
        public int NumberOfGenerations { get { return _numberOfGenerations; } set { _numberOfGenerations = value; } }

        /// <summary>
        /// Allow for this to be defined after ctor time, defaults to <see cref="FitnessFunction"/>
        /// </summary>
        public IFx<IState> FitnessFunction { get; set; }

        #region methods
        /// <summary>
        /// Does NOT use <see cref="IProblem.IsGoalTest"/> to return an <see cref="IState"/> but simply gets
        /// the <see cref="IState"/> which max'ed the <see cref="FitnessFunction"/> after <see cref="NumberOfGenerations"/>
        /// generations.
        /// </summary>
        /// <returns></returns>
        public IState Search()
        {
            var popLen = _wtFitness2State.Count;
            //go through the number of supplied generations
            for (var i = 0; i < _numberOfGenerations; i++)
            {
                //for each generation the population is replaced by children of more fit offspring
                var nextGeneration = new List<IState>();
                for (var j = 0; j < popLen; j++)
                {
                    var ps = PickFolks();
                    var child = _mutationFx(_reproductionFx(ps.Item1, ps.Item2));
                    nextGeneration.Add(child);
                }

                CalcWtFitness2State(nextGeneration);
            }

            //there can be only one.
            return _wtFitness2State.Select(x => x.Item2).ToDictionary(x => FitnessFunction.Eval(x)).First().Value;
        }

        /// <summary>
        /// We keep the population bound to a probablity of fitness 
        /// </summary>
        /// <param name="pop">
        /// </param>
        /// <example>
        /// <![CDATA[
        /// The population is represented as a coupled-pair of weighted fitness probablity
        /// to the the State
        /// 
        /// Weighted Prob     State
        /// 1                 s_1
        /// 0.7961956521      s_2
        /// 0.5448369565      s_3
        ///     ...           ...
        /// 0.1698369565      s_i-1 
        /// 0.0543478260      s_i
        /// 
        /// This way we can pick a parent with a simple 
        ///   .First(x => x.Item1 <= randProb).Item2
        /// ]]>
        /// </example>
        private void CalcWtFitness2State(IList<IState> pop)
        {
            pop.Shuffle();
            var calc = new List<Tuple<double, IState>>();
            var total = pop.Select(x => FitnessFunction.Eval(x)).Sum();
            var runningTotal = 0.0D;
            foreach (var s in pop)
            {
                runningTotal += FitnessFunction.Eval(s) / total;
                calc.Add(new Tuple<double, IState>(runningTotal, s));
            }
            _wtFitness2State = calc.OrderByDescending(x => x.Item1).ToList();
        }

        /// <summary>
        /// Picks two <see cref="IState"/> from the poplutaion weighed by thier 
        /// fitness
        /// </summary>
        /// <returns>Golly jee willikers Mister</returns>
        private Tuple<IState, IState> PickFolks()
        {
            var mom = _wtFitness2State.First(x => x.Item1 <= _myRand.NextDouble()).Item2;
            var pop = _wtFitness2State.First(x => x.Item1 <= _myRand.NextDouble()).Item2;
            return new Tuple<IState, IState>(mom, pop);
        }
        #endregion
    }
}
