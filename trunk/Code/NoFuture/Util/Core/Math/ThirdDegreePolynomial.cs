namespace NoFuture.Util.Core.Math
{
    public class ThirdDegreePolynomial : SecondDegreePolynomial
    {
        public ThirdDegreePolynomial(double a, double b, double c, double d) : base(b, c, d)
        {
            ThirdCoefficient = a;
        }

        public double ThirdCoefficient { get;}

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
