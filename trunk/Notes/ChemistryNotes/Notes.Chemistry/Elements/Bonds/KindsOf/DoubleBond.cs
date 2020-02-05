using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Chemistry.Elements.Bonds.KindsOf
{
    public class DoubleBond : BondDecorator
    {
        protected internal DoubleBond(IBond toDecorate) : base(toDecorate)
        {
        }
    }
}
