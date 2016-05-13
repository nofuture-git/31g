namespace NoFuture.Util.Math
{
    public class LinearEquation
    {
        public double Intercept { get; set; }
        public double Slope { get; set; }

        public virtual double SolveForY(double x)
        {
            return Slope*x + Intercept;
        }

        public virtual double SolveForX(double y)
        {
            if(Slope == 0.0D)
                return 0.0D;
            return (1/Slope)*y - (Intercept/Slope);
        }
    }
}
