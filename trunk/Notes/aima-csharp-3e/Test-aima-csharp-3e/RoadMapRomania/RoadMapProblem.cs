using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Agent;
using aima_csharp_3e.Search;
using aima_csharp_3e.Search.Framework;

namespace Test_aima_csharp_3e.RoadMapRomania
{
    public class RoadMapProblem : IProblem
    {
        public IState InitState { get { return City.GetAllCities().First(x => x.Name == "Arad"); } }
        public object Goal { get { return City.GetAllCities().First(x => x.Name == "Bucharest"); } }

        public IEnumerable<Tuple<IAction, IState>> GetSuccessor(IState state)
        {
            var atCity = state as City;
            if(atCity == null)
                throw new ArgumentException("This is not a City");
            switch (atCity.Name)
            {
                case "Oradea":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(71), new City("Zerind")),
                        new Tuple<IAction, IState>(new Drive(151), new City("Sibiu"))
                    };

                case "Zerind":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(71), new City("Oradea")),
                        new Tuple<IAction, IState>(new Drive(75), new City("Arad"))
                    };

                case "Arad":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(140), new City("Sibiu")),
                        new Tuple<IAction, IState>(new Drive(75), new City("Zerind")),
                        new Tuple<IAction, IState>(new Drive(118), new City("Timisoara")),
                    };

                case "Timisoara":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(111), new City("Lugoj")),
                        new Tuple<IAction, IState>(new Drive(118), new City("Arad")),
                    };

                case "Lugoj":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(111), new City("Timisoara")),
                        new Tuple<IAction, IState>(new Drive(70), new City("Mehadia")),
                    };

                case "Mehadia":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(75), new City("Drobeta")),
                        new Tuple<IAction, IState>(new Drive(70), new City("Lugoj")),
                    };

                case "Drobeta":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(75), new City("Mehadia")),
                        new Tuple<IAction, IState>(new Drive(120), new City("Craiova")),
                    };

                case "Sibiu":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(151), new City("Oradea")),
                        new Tuple<IAction, IState>(new Drive(140), new City("Arad")),
                        new Tuple<IAction, IState>(new Drive(99), new City("Fagaras")),
                        new Tuple<IAction, IState>(new Drive(80), new City("Rimnicu Vilcea")),
                    };

                case "Rimnicu Vilcea":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(146), new City("Craiova")),
                        new Tuple<IAction, IState>(new Drive(97), new City("Pitesti")),
                        new Tuple<IAction, IState>(new Drive(80), new City("Sibiu")),
                    };

                case "Craiova":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(146), new City("Rimnicu Vilcea")),
                        new Tuple<IAction, IState>(new Drive(138), new City("Pitesti")),
                        new Tuple<IAction, IState>(new Drive(120), new City("Drobeta")),
                    };

                case "Fagaras":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(99), new City("Sibiu")),
                        new Tuple<IAction, IState>(new Drive(211), new City("Bucharest")),
                    };

                case "Pitesti":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(97), new City("Rimnicu Vilcea")),
                        new Tuple<IAction, IState>(new Drive(138), new City("Craiova")),
                        new Tuple<IAction, IState>(new Drive(101), new City("Bucharest")),
                    };

                case "Bucharest":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(211), new City("Fagaras")),
                        new Tuple<IAction, IState>(new Drive(101), new City("Pitesti")),
                        new Tuple<IAction, IState>(new Drive(90), new City("Giurgiu")),
                        new Tuple<IAction, IState>(new Drive(85), new City("Urziceni")),
                    };

                case "Giurgiu":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(90), new City("Bucharest")),
                    };

                case "Urziceni":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(142), new City("Vaslui")),
                        new Tuple<IAction, IState>(new Drive(98), new City("Hirsova")),
                        new Tuple<IAction, IState>(new Drive(85), new City("Bucharest")),
                    };

                case "Neamt":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(87), new City("Iasi")),
                    };

                case "Iasi":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(87), new City("Neamt")),
                        new Tuple<IAction, IState>(new Drive(92), new City("Vaslui")),
                    };

                case "Vaslui":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(142), new City("Urziceni")),
                        new Tuple<IAction, IState>(new Drive(92), new City("Iasi")),
                    };

                case "Hirsova":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(98), new City("Urziceni")),
                        new Tuple<IAction, IState>(new Drive(86), new City("Eforie")),
                    };

                case "Eforie":
                    return new List<Tuple<IAction, IState>>
                    {
                        new Tuple<IAction, IState>(new Drive(86), new City("Hirsova")),
                    };

            }
            throw new NotImplementedException(string.Format("No such city '{0}' was found", atCity.Name));
        }

        public bool IsGoalTest(IState state)
        {
            return state.Equals(Goal);
        }

        public double GetPathCost(double costUpToState1, IState state1, IAction action, IState state2)
        {
            //for this cost is only in the drive distance
            var drive = action as Drive ?? new Drive(0);
            return costUpToState1 + drive.Distance;
        }

        public double GetValue()
        {
            throw new NotImplementedException();
        }
    }

    public class StraightLineDistance : NodeFx
    {
        public StraightLineDistance(): base(node => GetDistance(node)) { }

        internal static double GetDistance(Node n)
        {
            var state = n?.State;
            var city = state as City;
            if (city == null)
                return int.MaxValue; //0 is GOAL! - so return something far away.
            switch (city.Name)
            {
                case "Arad":
                    return 366.0D;

                case "Bucharest":
                    return 0.0D;

                case "Craiova":
                    return 160.0D;

                case "Drobeta":
                    return 242.0D;

                case "Eforie":
                    return 161.0D;

                case "Fagaras":
                    return 176.0D;

                case "Giurgiu":
                    return 77.0D;

                case "Hirsova":
                    return 151.0D;

                case "Iasi":
                    return 226.0D;

                case "Lugoj":
                    return 244.0D;

                case "Mehadia":
                    return 241.0D;

                case "Neamt":
                    return 234.0D;

                case "Oradea":
                    return 380.0D;

                case "Pitesti":
                    return 100.0D;

                case "Rimnicu Vilcea":
                    return 193.0D;

                case "Sibiu":
                    return 253.0D;

                case "Timisoara":
                    return 329.0D;

                case "Urziceni":
                    return 80.0D;

                case "Vaslui":
                    return 199.0D;

                case "Zerind":
                    return 374.0D;
            }

            throw new NotImplementedException(string.Format("No such city '{0}' was found", city.Name));
        }
    }
}
