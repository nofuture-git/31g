using System;
using System.Globalization;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Domus.Pneuma
{
    [Serializable]
    public class Dimension
    {
        public Dimension(double z, double stdDev)
        {
            Zscore = z;
            StdDev = stdDev;
        }

        public Dimension(double z)
        {
            Zscore = z;
        }

        public Dimension()
        {
            Zscore = Etx.RandomDouble();
        }

        public override int GetHashCode()
        {
            return Zscore.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var z = obj as Dimension;
            if (z == null)
                return false;
            return Math.Abs(Zscore - z.Zscore) <= 0.0000000;
        }

        public override string ToString()
        {
            return Zscore.ToString(CultureInfo.InvariantCulture);
        }

        public double Zscore { get; }
        public double StdDev { get; } = 0.33334D;
    }
}