namespace NoFuture.Util.Math
{
    public class LinearEquation : IEquation
    {
        public LinearEquation() { }

        public LinearEquation(double intercept, double slope)
        {
            Intercept = intercept;
            Slope = slope;
        }

        public double Intercept { get; set; }
        public double Slope { get; set; }

        public virtual double SolveForY(double x)
        {
            return System.Math.Round(Slope*x + Intercept,5);
        }

        public virtual double SolveForX(double y)
        {
            if(System.Math.Abs(Slope) < 0.0000001D)
                return 0.0D;
            return (1/Slope)*y - (Intercept/Slope);
        }

        public override string ToString()
        {
            return $"f(x) = {Slope}x + {Intercept}";
        }
    }
}
