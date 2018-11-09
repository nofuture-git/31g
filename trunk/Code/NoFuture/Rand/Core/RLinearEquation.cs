using System;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Core
{
    [Serializable]
    public class RLinearEquation : LinearEquation
    {
        public RLinearEquation(double slope, double intercept) : base(slope, intercept){ }

        /// <summary>
        /// Upon call to <see cref="SolveForY"/> any x value greater than this will be replaced with this value
        /// </summary>
        public double? MaxX { get; set; }

        /// <summary>
        /// Upon call to <see cref="SolveForY"/> any x value less than this will be replaced with this value
        /// </summary>
        public double? MinX { get; set; }

        /// <summary>
        /// Controls how much randomness is possible around the average
        /// </summary>
        public double StdDev { get; set; }

        /// <summary>
        /// Same as its super class only adds deviation so that recurrent invocations will 
        /// not result in the same value.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public override double SolveForY(double x)
        {
            x = MaxX != null && MaxX.Value < x ? MaxX.Value : x;
            x = MinX != null && MinX.Value > x ? MinX.Value : x;

            var mean = base.SolveForY(x);

            return StdDev > 0 ? Etx.RandomValueInNormalDist(mean, StdDev) : mean;
        }
    }
}
