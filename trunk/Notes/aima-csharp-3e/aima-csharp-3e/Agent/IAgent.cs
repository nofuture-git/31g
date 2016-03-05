using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aima_csharp_3e.Agent
{
    /// <summary>
    /// A description of the possible actions available to the agent
    /// given a particular state.
    /// </summary>
    public interface IAgent : IEnvObject
    {
        IAction Exec(IPercept p);
    }
}
