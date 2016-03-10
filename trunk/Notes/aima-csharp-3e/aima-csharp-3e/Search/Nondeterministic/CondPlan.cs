using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Agent;

namespace aima_csharp_3e.Search.Nondeterministic
{
    public class CondPlan
    {
        public IState CurrentPosition { get; set; }
        public IAction Action { get; set; }
        public IEnumerable<IState> ResultStates { get; set; }
        public CondPlan Next { get; set; }
    }
}
