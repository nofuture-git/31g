namespace NoFuture.Util.Core.Math
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

        /// <summary>
        /// Parse a string in the format of [intercept],[slope] 
        /// where both intercept and slope may be parsed to doubles
        /// and are joined by a comma.
        /// </summary>
        /// <param name="csv"></param>
        /// <param name="lq"></param>
        /// <returns></returns>
        public static bool TryParse(string csv, out LinearEquation lq)
        {
            lq = null;
            if (string.IsNullOrWhiteSpace(csv) || !csv.Contains(","))
                return false;
            var interceptStr = csv.Split(',')[0];
            var slopeStr = csv.Split(',')[1];

            double intercept;
            double slope;
            if (double.TryParse(interceptStr, out intercept) && double.TryParse(slopeStr, out slope))
            {
                lq = new LinearEquation { Intercept = intercept, Slope = slope };
            }
            return lq != null;
        }
    }
}
