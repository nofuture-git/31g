using System;

namespace NoFuture.Rand
{
    [Serializable]
    public class RLinearEquation : Util.Math.LinearEquation
    {
        public double StdDev { get; set; }

        /// <summary>
        /// Same as its super class only adds deviation so that recurrent invocations will 
        /// not result in the same value.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public override double SolveForY(double x)
        {
            var mean = base.SolveForY(x);

            return StdDev > 0 ? Etx.RandomValueInNormalDist(mean, StdDev) : mean;
        }
    }
}
