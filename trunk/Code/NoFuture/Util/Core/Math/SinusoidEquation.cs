﻿namespace NoFuture.Util.Core.Math
{
    public class SinusoidEquation : IEquation
    {
        private double _amp;

        public double Amplitude
        {
            get
            {
                if (System.Math.Abs(_amp) < 0.0000001)
                    _amp = 1.0D;
                return _amp;
            }
            set
            {
                _amp = value;
            }
        }

        public double Frequency { get; set; }
        public double Phase { get; set; }
        /// <summary>
        /// Optional addition to have the wave orbit around this value
        /// </summary>
        public double CenterAxis { get; set; }
        public double SolveForY(double x)
        {
            return Amplitude * System.Math.Sin(2*System.Math.PI*Frequency*x + Phase) + CenterAxis;
        }

        public override string ToString()
        {
            return $"f(x) = {Amplitude} * sin(2π{Frequency} + {Phase})";
        }
    }
}
