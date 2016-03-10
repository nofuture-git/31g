using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Agent;
using aima_csharp_3e.Search;
using aima_csharp_3e.Search.Framework;
using aima_csharp_3e.Search.Nondeterministic;

namespace Test_aima_csharp_3e.ErraticVacuumWorld
{


    public class ErraticVacuumProblem : NdProblemBase
    {
        public static Position[] Positions = new[]
            {
                new Position
                {
                    LeftIsDirty = true,
                    RightIsDirty = true,
                    MyPosition = new Left(),
                    Id = 1
                },
                new Position
                {
                    LeftIsDirty = true,
                    RightIsDirty = true,
                    MyPosition = new Right(),
                    Id = 2
                },
                new Position
                {
                    LeftIsDirty = true,
                    RightIsDirty = false,
                    MyPosition = new Left(),
                    Id = 3
                },
                new Position
                {
                    LeftIsDirty = true,
                    RightIsDirty = false,
                    MyPosition = new Right(),
                    Id = 4
                },
                new Position
                {
                    LeftIsDirty = false,
                    RightIsDirty = true,
                    MyPosition = new Left(),
                    Id = 5
                },
                new Position
                {
                    LeftIsDirty = false,
                    RightIsDirty = true,
                    MyPosition = new Right(),
                    Id = 6
                },
                new Position
                {
                    LeftIsDirty = false,
                    RightIsDirty = false,
                    MyPosition = new Left(),
                    Id = 7
                },
                new Position
                {
                    LeftIsDirty = false,
                    RightIsDirty = true,
                    MyPosition = new Right(),
                    Id = 8
                }
            };

        public override IState InitState { get { return GetPositionById(1); } }

        public override bool IsGoalTest(IState s)
        {
            var pos = s as Position;
            if (pos == null)
                return false;
            return !pos.RightIsDirty && !pos.LeftIsDirty;
        }

        public override double GetPathCost(double costUpToState1, IState state1, IAction action, IState state2)
        {
            return costUpToState1 + 1;
        }

        /// <summary>
        /// this is an example of a <see cref="CondPlan"/> based on Fig. 4.10
        /// </summary>
        public CondPlan Example()
        {
            var firstStep = new CondPlan
            {
                CurrentPosition = InitState,
                Action = new Suck(),
                Next = null,
                ResultStates =
                    new List<IState>
                    {
                        GetPositionById(7),
                        GetPositionById(5)
                    }
            };

            var secondStep = new CondPlan
            {
                CurrentPosition = GetPositionById(5),
                Action = new Right(),
                Next = firstStep,
                ResultStates =
                    new List<IState>
                    {
                        GetPositionById(6)
                    }

            };

            return new CondPlan
            {
                CurrentPosition = GetPositionById(6),
                Action = new Suck(),
                Next = secondStep,
                ResultStates =
                    new List<IState>
                    {
                        GetPositionById(8)
                    }
            };
            
        }

        public Position GetPositionById(int id)
        {
            return Positions.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Sec. 4.3.1 
        /// * When applied to a dirty square the action cleans the square and sometimes cleans up dirt
        /// in an adjacent square, too.
        /// * When applied to a clean square the action sometimes deposits dirt on the carpet
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <remarks>
        /// This differs from the Road Map Problem because an action results in more than one or more
        /// possiable states (the OR-NODE).
        /// </remarks>
        public override IEnumerable<Tuple<IAction, IEnumerable<IState>>> GetSuccessor(IState state)
        {
            var atPosition = state as Position;
            if(atPosition == null)
                throw new ArgumentException("This is not a valid position");

            if (atPosition.Equals(GetPositionById(1)))
            {
                return new List<Tuple<IAction, IEnumerable<IState>>>
                {
                    new Tuple<IAction, IEnumerable<IState>>(new Suck(),
                        new List<IState>
                        {
                            GetPositionById(7),
                            GetPositionById(5)
                        }),
                    new Tuple<IAction, IEnumerable<IState>>(new Right(),
                        new List<IState>
                        {
                            GetPositionById(2)
                        })
                };
            }

            if (
                atPosition.Equals(GetPositionById(2)))
            {
                return new List<Tuple<IAction, IEnumerable<IState>>>
                {
                    new Tuple<IAction, IEnumerable<IState>>(new Left(),
                        new List<IState> {GetPositionById(1)}),
                    new Tuple<IAction, IEnumerable<IState>>(new Suck(),
                        new List<IState>
                        {
                            GetPositionById(8),
                            GetPositionById(4)
                        }),
                };
            }

            if (
                atPosition.Equals(GetPositionById(3)))
            {
                return new List<Tuple<IAction, IEnumerable<IState>>>
                {
                    new Tuple<IAction, IEnumerable<IState>>(new Right(), 
                        new List<IState> {GetPositionById(4)}),
                    new Tuple<IAction, IEnumerable<IState>>(new Suck(),
                        new List<IState>
                        {
                            GetPositionById(7)
                        }),
                };
            }

            if (
                atPosition.Equals(GetPositionById(4)))
            {
                return new List<Tuple<IAction, IEnumerable<IState>>>
                {
                    new Tuple<IAction, IEnumerable<IState>>(new Left(),
                        new List<IState> {GetPositionById(3)}),
                    new Tuple<IAction, IEnumerable<IState>>(new Suck(),
                        new List<IState>
                        {
                            GetPositionById(4),
                            GetPositionById(2),
                        }),
                };

            }

            if (
                atPosition.Equals(GetPositionById(5)))
            {
                return new List<Tuple<IAction, IEnumerable<IState>>>
                {
                    new Tuple<IAction, IEnumerable<IState>>(new Suck(),
                        new List<IState>
                        {
                            GetPositionById(5),
                            GetPositionById(1)
                        }),
                    new Tuple<IAction, IEnumerable<IState>>(new Right(),
                        new List<IState>
                        {
                            GetPositionById(6)
                        })
                };
            }

            if (
                atPosition.Equals(GetPositionById(6)))
            {
                return new List<Tuple<IAction, IEnumerable<IState>>>
                {
                    new Tuple<IAction, IEnumerable<IState>>(new Suck(),
                        new List<IState>
                        {
                            GetPositionById(8),
                        }),
                    new Tuple<IAction, IEnumerable<IState>>(new Left(),
                        new List<IState>
                        {
                            GetPositionById(5)
                        })
                };
            }

            if (
                atPosition.Equals(GetPositionById(7)))
            {
                return new List<Tuple<IAction, IEnumerable<IState>>>
                {
                    new Tuple<IAction, IEnumerable<IState>>(new Suck(),
                        new List<IState>
                        {
                            GetPositionById(7),
                            GetPositionById(3),
                        }),
                    new Tuple<IAction, IEnumerable<IState>>(new Right(),
                        new List<IState>
                        {
                            GetPositionById(8)
                        })
                };
            }

            if (
                atPosition.Equals(GetPositionById(8)))
            {
                return new List<Tuple<IAction, IEnumerable<IState>>>
                {
                    new Tuple<IAction, IEnumerable<IState>>(new Suck(),
                        new List<IState>
                        {
                            GetPositionById(8),
                            GetPositionById(6)
                        }),
                    new Tuple<IAction, IEnumerable<IState>>(new Left(),
                        new List<IState>
                        {
                            GetPositionById(7)
                        }),
                };
            }

            throw new NotImplementedException();
        }
    }
}
