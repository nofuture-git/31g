namespace NoFuture.Util.Math
{
    public class SinusoidEquation : IEquation
    {
        public double? Amplitude { get; set; }
        public double Frequency { get; set; }
        public double Phase { get; set; }

        public double SolveForY(double x)
        {
            var amp = Amplitude.GetValueOrDefault(1.0D);
            return amp * System.Math.Sin(2*System.Math.PI*Frequency + Phase);
        }
    }
}
