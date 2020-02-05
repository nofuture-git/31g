using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Chemistry.Elements.Bonds.KindsOf
{
    public class SingleBond : BondDecorator
    {
        protected internal SingleBond(IBond toDecorate) : base(toDecorate)
        {
        }
    }
}
