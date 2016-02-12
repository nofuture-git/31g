using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Util.Math
{
    public class NaturalLogEquation
    {
        public double Intercept { get; set; }
        public double Slope { get; set; }

        public double SolveForY(double x)
        {
            return (Slope*(System.Math.Log(x))) + Intercept;
        }

        public double SolveForX(double y)
        {
            if (Slope == 0.0D)
                return 0.0D;
            var yy = y - Intercept;
            yy = yy / Slope;
            return System.Math.Pow(System.Math.E, yy);

        }
    }
}
