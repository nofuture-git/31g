using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Chemistry.Elements.Bonds
{
    public abstract class CovalentBase : BondBase
    {
        protected CovalentBase(IElement atom1, IElement atom2) : base(atom1, atom2)
        {
        }
    }
}
