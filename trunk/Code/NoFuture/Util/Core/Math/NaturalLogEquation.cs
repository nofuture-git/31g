namespace NoFuture.Util.Core.Math
{
    public class NaturalLogEquation : IEquation
    {
        public double Intercept { get; set; }
        public double Slope { get; set; }

        public virtual double SolveForY(double x)
        {
            return Slope*System.Math.Log(x) + Intercept;
        }

        public virtual double SolveForX(double y)
        {
            return System.Math.Abs(Slope) < 0.0000001D 
                ? 0.0D 
                : System.Math.Pow(System.Math.E, (y - Intercept)/Slope);
        }

        public override string ToString()
        {
            return $"f(x) = {Slope}*ln(x) + {Intercept}";
        }
    }
}
