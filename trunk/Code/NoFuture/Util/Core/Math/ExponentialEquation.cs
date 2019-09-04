namespace NoFuture.Util.Core.Math
{
    public class ExponentialEquation : IEquation
    {
        /// <summary>
        /// In the form of f(x) = a * x^n this is &quot;a&quot;
        /// </summary>
        public double ConstantValue { get; set; }

        /// <summary>
        /// In the form of f(x) = a * x^n this is &quot;n&quot;
        /// </summary>
        public double Power { get; set; }

        public double SolveForY(double x)
        {
            return ConstantValue*System.Math.Pow(x, Power);
        }

        public double SolveForX(double y)
        {
            return System.Math.Pow((y/ConstantValue), (1/Power));
        }

        public override string ToString()
        {
            return $"f(x) = {ConstantValue} * x^{Power}";
        }
    }
}
