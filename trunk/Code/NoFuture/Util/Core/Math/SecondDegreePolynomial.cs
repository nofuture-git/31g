using System;

namespace NoFuture.Util.Core.Math
{
    public class SecondDegreePolynomial : LinearEquation
    {
        public SecondDegreePolynomial(double a, double b, double c) : base(b, c)
        {
            SecondCoefficient = a;
        }
        public double SecondCoefficient { get; }

        public override double SolveForY(double x)
        {
            return System.Math.Round((SecondCoefficient * System.Math.Pow(x,2)) + (Slope * x) + Intercept, 5);
        }

        public override double SolveForX(double y)
        {
            //TODO implement this someday
            throw new NotImplementedException();
        }

        public override double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return SecondCoefficient;
                    case 1:
                    case 2:
                        return base[index];
                    default:
                        return 0D;
                }
            }
        }

        public override IEquation Clone()
        {
            return new SecondDegreePolynomial(SecondCoefficient, Slope, Intercept);
        }

        public override string ToString()
        {
            return $"f(x) = {SecondCoefficient}x² + {Slope}x + {Intercept}";
        }

        public override int GetHashCode()
        {
            return SecondCoefficient.GetHashCode() + base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SecondDegreePolynomial))
                return false;

            var nfv = (SecondDegreePolynomial)obj;

            var v0Eq = nfv.SecondCoefficient - SecondCoefficient < CloseEnough;
            var v1Eq = nfv.Slope - Slope < CloseEnough;
            var v2Eq = nfv.Intercept - Intercept < CloseEnough;
            return v0Eq && v1Eq && v2Eq;
        }
    }
}
