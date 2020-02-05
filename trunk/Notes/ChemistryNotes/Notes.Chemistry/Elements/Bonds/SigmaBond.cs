using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Chemistry.Elements.Bonds
{
    public class SigmaBond : CovalentBase
    {
        public SigmaBond(IElement atom1, IElement atom2) : base(atom1, atom2)
        {
        }
    }

    public class PiBond : CovalentBase
    {
        public PiBond(IElement atom1, IElement atom2) : base(atom1, atom2)
        {
        }
    }
}
