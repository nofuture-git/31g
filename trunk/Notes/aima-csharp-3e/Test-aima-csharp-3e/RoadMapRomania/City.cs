using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Agent;

namespace Test_aima_csharp_3e.RoadMapRomania
{
    public class City : IState
    {
        private readonly string _name;
        public string Name { get {return _name;} }

        public City(string name)
        {
            _name = name;
        }

        public override bool Equals(object obj)
        {
            var rCity = obj as City;
            return rCity != null && Name.Equals(rCity.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// <![CDATA[
        /// $cities = @(
        /// "Oradea",
        /// "Zerind",
        /// "Arad",
        /// "Timisoara",
        /// "Lugoj",
        /// "Mehadia",
        /// "Drobeta",
        /// "Sibiu",
        /// "Rimnicu Vilcea",
        /// "Craiova",
        /// "Fagaras",
        /// "Pitesti",
        /// "Bucharest",
        /// "Giurgiu",
        /// "Urziceni",
        /// "Neamt",
        /// "Iasi",
        /// "Vaslui",
        /// "Hirsova",
        /// "Eforie")
        /// 
        /// $cities | % {$c = $_; @"
        ///      case "$c":
        ///          break;
        /// 
        /// "@
        /// }
        /// ]]>
        /// </example>
        public static List<City> GetAllCities()
        {
            var cities = new List<City>()
            {
                new City("Oradea"),
                new City("Zerind"),
                new City("Arad"),
                new City("Timisoara"),
                new City("Lugoj"),
                new City("Mehadia"),
                new City("Drobeta"),
                new City("Sibiu"),
                new City("Rimnicu Vilcea"),
                new City("Craiova"),
                new City("Fagaras"),
                new City("Pitesti"),
                new City("Bucharest"),
                new City("Giurgiu"),
                new City("Urziceni"),
                new City("Neamt"),
                new City("Iasi"),
                new City("Vaslui"),
                new City("Hirsova"),
                new City("Eforie")
            };
            return cities;
        }
    }


}
