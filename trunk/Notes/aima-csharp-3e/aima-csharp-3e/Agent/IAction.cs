using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aima_csharp_3e.Agent
{
    public interface IAction
    {
        bool IsNoOp();
    }
    public class NoOpAction : IAction
    {
        public bool IsNoOp()
        {
            return true;
        }
    }
}
