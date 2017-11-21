using System;

namespace NoFuture.Util.Core.Math
{
    public class SecondDegreePolynomial : LinearEquation
    {
        public double SecondCoefficient { get; set; }

        public override double SolveForY(double x)
        {
            return System.Math.Round((SecondCoefficient * System.Math.Pow(x,2)) + (Slope * x) + Intercept, 5);
        }

        public override double SolveForX(double y)
        {
            //TODO implement this someday
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"f(x) = {SecondCoefficient}x² {Slope}x + {Intercept}";
        }
    }
}
