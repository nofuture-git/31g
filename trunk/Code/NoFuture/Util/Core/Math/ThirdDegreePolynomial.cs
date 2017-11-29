namespace NoFuture.Util.Core.Math
{
    public class ThirdDegreePolynomial : SecondDegreePolynomial
    {
        public double ThirdCoefficient { get; set; }

        public override double SolveForY(double x)
        {
            return System.Math.Round(
                ThirdCoefficient * System.Math.Pow(x, 3) + SecondCoefficient * System.Math.Pow(x, 2) + Slope * x +
                Intercept, 5);
        }

        public override string ToString()
        {
            return $"f(x) ={ThirdCoefficient}x³ + {SecondCoefficient}x² + {Slope}x + {Intercept}";
        }
    }
}
