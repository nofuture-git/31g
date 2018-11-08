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

        public override double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return ThirdCoefficient;
                    case 1:
                    case 2:
                    case 3:
                        return base[index];
                    default:
                        return 0D;
                }
            }
        }

        public override IEquation Clone()
        {
            return new ThirdDegreePolynomial(ThirdCoefficient, SecondCoefficient, Slope, Intercept);
        }

        public override string ToString()
        {
            return $"f(x) ={ThirdCoefficient}x³ + {SecondCoefficient}x² + {Slope}x + {Intercept}";
        }

        public override int GetHashCode()
        {
            return ThirdCoefficient.GetHashCode() + base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ThirdDegreePolynomial))
                return false;

            var nfv = (ThirdDegreePolynomial)obj;

            var v0Eq = nfv.ThirdCoefficient - ThirdCoefficient < CloseEnough;
            var v1Eq = nfv.SecondCoefficient - SecondCoefficient < CloseEnough;
            var v2Eq = nfv.Slope - Slope < CloseEnough;
            var v3Eq = nfv.Intercept - Intercept < CloseEnough;
            return v0Eq && v1Eq && v2Eq && v3Eq;
        }
    }
}
