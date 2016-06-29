﻿namespace NoFuture.Util.Math
{
    public class NaturalLogEquation : IEquation
    {
        public double Intercept { get; set; }
        public double Slope { get; set; }

        public virtual double SolveForY(double x)
        {
            return (Slope*(System.Math.Log(x))) + Intercept;
        }

        public virtual double SolveForX(double y)
        {
            if (System.Math.Abs(Slope) < 0.0000000D)
                return 0.0D;
            var yy = y - Intercept;
            yy = yy / Slope;
            return System.Math.Pow(System.Math.E, yy);

        }
    }
}
