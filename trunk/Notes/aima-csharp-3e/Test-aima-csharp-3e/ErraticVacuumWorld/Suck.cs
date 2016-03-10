using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Agent;

namespace Test_aima_csharp_3e.ErraticVacuumWorld
{
    public class Suck : IAction
    {
        public bool IsNoOp()
        {
            return false;
        }

    }
}
