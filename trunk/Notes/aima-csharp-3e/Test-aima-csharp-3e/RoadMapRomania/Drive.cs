using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Agent;

namespace Test_aima_csharp_3e.RoadMapRomania
{
    public class Drive : IAction
    {
        private readonly int _distance;

        public Drive(int distance)
        {
            _distance = distance;
        }
        public bool IsNoOp()
        {
            return false;
        }

        public int Distance
        {
            get { return _distance; }
        }
    }
}
