using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Agent;

namespace Test_aima_csharp_3e.ErraticVacuumWorld
{

    public class Position : IState
    {
        public bool LeftIsDirty { get; set; }
        public bool RightIsDirty { get; set; }
        public IAction MyPosition { get; set; }
        public int Id { get; set; }

        public bool IsClean()
        {
            return !LeftIsDirty && !RightIsDirty;
        }

        public override bool Equals(object obj)
        {
            var p = obj as Position;
            if (p == null)
                return false;
            return p.Id == Id;
        }
    }
}
