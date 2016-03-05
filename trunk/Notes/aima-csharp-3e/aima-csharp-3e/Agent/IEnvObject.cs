using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aima_csharp_3e.Agent
{
    /// <summary>
    /// This represents any physical object that can appear in an Environment.
    /// </summary>
    public interface IEnvObject
    {
        bool IsAlive { get; set; }
    }
}
